using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Feeds input into player and its guns
    /// </summary>
    public class PlayerInputManager : MonoBehaviour
    {
        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;

        //[Header("Keyboard + Mouse Controls")]
        //[SerializeField]
        //private KeyCode kbmShoot = KeyCode.Mouse0;
        //[SerializeField]
        //private KeyCode kbmPause = KeyCode.Escape;
        //[SerializeField]
        //private KeyCode kbmTimeMechanic = KeyCode.Mouse1;
        //[SerializeField]
        //private KeyCode kbmDashMechanic = KeyCode.Space;

        [SerializeField]
        private List<KeyCode> weaponSwitches = new List<KeyCode>();
        private List<System.Type> weapons = new List<System.Type>();


        //[Header("Joystick Controls")]
        //[SerializeField]
        //private KeyCode controllerShoot = KeyCode.Joystick1Button5; // Rb
        //[SerializeField]
        //private KeyCode controllerPause = KeyCode.Joystick1Button7; // Start
        //[SerializeField]
        //private KeyCode controllerTimeMechanic = KeyCode.Joystick1Button4; // Lb
        //[SerializeField]
        //private KeyCode controllerDashMechanic = KeyCode.Joystick1Button0; // A


        private Plane mousePlane; // Plane to track the mouse position on screen.
        private Vector2 mousePos;

        private PlayerController player;
        private new Rigidbody rigidbody;
        private GunController gunController;
        private WeaponUISwitch weaponUISwitch;
        private PlayerUIController pUI;

        private TimeMechanic.LocalTimeDilation myTime;
        private TimeMechanic.TimeMechanic timeMechanic;

        private Dash dashMechanic;

        private bool allowInput = true;
        private float forceMoveDistanceCounter = 0.0f;
        private float forceMoveDistanceTarget = 0.0f;

        private Vector3 directionVector;

        private enum ControlMethod { KBM, CONTROLLER };
        private ControlMethod control = ControlMethod.KBM;

        private bool paused = false; // Is player input halted?

        [Header("Debugging")]
        [SerializeField]
        private bool testInput = false; // Used to figure out keycodes without looking them up,
                                       // should be true only when discovering buttons to map.
        [SerializeField]
        private bool debugging = false;

        private int currentWeapon;


        // Use this for initialization

        void Start()
        {
            player = GetComponent<PlayerController>();
            rigidbody = GetComponent<Rigidbody>();
            pUI = GetComponent<PlayerUIController>();
            gunController = transform.Find("GunPrimary").GetComponent<GunController>();
            weaponUISwitch = FindObjectOfType<WeaponUISwitch>();

            currentWeapon = 0;

            mousePlane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));

            initWeaponTypes();

            myTime = GetComponent<TimeMechanic.LocalTimeDilation>();
            dashMechanic = GetComponent<Dash>();
        }

        /// <summary>
        /// Adds iterable weapon types to a list
        /// </summary>
        private void initWeaponTypes()
        {
            weapons.Add(typeof(Weapon.Gun.Handgun));
            weapons.Add(typeof(Weapon.Gun.Shotgun));
            weapons.Add(typeof(Weapon.Gun.MachineGun));
            weapons.Add(typeof(Weapon.Gun.EXDHandgun));
            weapons.Add(typeof(Weapon.Gun.NonTimeEffectingGun));
        }

        // Update is called once per frame
        void Update()
        {
            if (testInput)
                printKeys();

            control = getControlMethod();

            if (debugging)
                Debug.Log("Control method: " + control);

            actions();

            if (paused)
                return;

            move();
            turn(control);
        }

        /// <summary>
        /// Gets the position of the mouse in world space.
        /// </summary>
        /// <returns>Position of mouse in world space.</returns>
        public Vector3 getMousePos()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            float intersect = 0.0f;
            mousePlane.Raycast(camRay, out intersect);

            if (debugging)
                Debug.DrawLine(Camera.main.transform.position, camRay.GetPoint(intersect), Color.red);

            return camRay.GetPoint(intersect);
        }

        /// <summary>
        /// Moves the player based on its current direction vector
        /// </summary>
        void move()
        {
            if (allowInput)
                directionVector = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f, UnityEngine.Input.GetAxisRaw("Vertical")).normalized;

            Vector3 movement = directionVector * player.moveSpeed * myTime.getDelta();
            rigidbody.transform.Translate(movement, Space.World);
            rigidbody.MovePosition(transform.position + movement);
            rigidbody.velocity = movement;

            if (!allowInput)
            {
                forceMoveDistanceCounter += movement.magnitude;

                if (forceMoveDistanceCounter >= forceMoveDistanceTarget)
                {
                    forceMoveDistanceCounter = 0.0f;
                    forceMoveDistanceTarget = 0.0f;
                    allowInput = true;

                    GetComponent<HealthManager.PlayerHealthManager>().setGodmode(true, 1.5f);
                }
            }
        }

        /// <summary>
        /// Forces a player to move in a direction for a set distance
        /// </summary>
        /// <param name="dir">Direction to move in.</param>
        /// <param name="distance">Distance to travel before stopping.</param>
        public void forceMove(Vector3 dir, float distance)
        {
            if (dir == Vector3.zero)
                return;

            allowInput = false;
            forceMoveDistanceTarget = distance;

            directionVector = dir;

            GetComponent<HealthManager.PlayerHealthManager>().setGodmode(true);
        }

        /// <summary>
        /// Turns player to look at mouse pos
        /// </summary>
        void turn(ControlMethod controlMethod)
        {
            if (controlMethod == ControlMethod.CONTROLLER)
            {
                float tolerance = 0.25f;
                Vector3 playerDirection = Vector3.right * UnityEngine.Input.GetAxisRaw("ShootHorizontal") + Vector3.forward * -UnityEngine.Input.GetAxisRaw("ShootVertical");

                if (playerDirection.magnitude > tolerance)
                    transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }
            else if (controlMethod == ControlMethod.KBM)
            {
                Vector3 target = getMousePos() - transform.position; // Get direction vector from player to target

                if (target.magnitude < 1.5f) // If direction vector too short, extend it 
                {
                    target.Normalize();
                    target *= 1.5f;
                }

                target += transform.position; // Apply this position to altered direction vector to produce new target position

                Vector3 firePoint = transform.Find("GunPrimary").transform.Find("Body").position;
                Vector3 gunToTarget = target - firePoint; // Get direction vector from gun to target
                gunToTarget.y = 0;
                gunToTarget.Normalize();

                float turnAngle = Vector3.SignedAngle(transform.forward, gunToTarget, Vector3.up); // Calculate angle between gun forward and target dir

                if (debugging)
                {
                    Debug.DrawLine(getMousePos() - Vector3.up * 5, getMousePos() + Vector3.up * 5, Color.red);
                    Debug.DrawRay(firePoint, transform.forward);
                    Debug.DrawRay(firePoint, gunToTarget);
                }

                transform.Rotate(Vector3.up, turnAngle);
            }
        }

        //////////////////////
        /// Input Checking ///
        //////////////////////

        /// <summary>
        /// Responsible for carrying out all other input actions
        /// </summary>
        private void actions()
        {
            if (inputCheck_Pause())
                return; // Return if paused

            inputCheck_Shoot();

            inputCheck_WeaponSwitch();

            inputCheck_TimeMechanic();

            inputCheck_Dash();
        }

        /// <summary>
        /// Used only to pause/unpause the game. 
        /// 
        /// Allows input checking even when game is paused.
        /// </summary>
        private bool inputCheck_Pause()
        {
            //if (Input.GetKeyDown(kbmPause) || Input.GetKeyDown(controllerPause))
            if (Input.GetButtonDown("Pause"))
                pause();

            return paused;
        }

        /// <summary>
        /// Checks if the player is trying to shoot
        /// </summary>
        private void inputCheck_Shoot()
        {
            //if (Input.GetKey(kbmShoot) || Input.GetKey(controllerShoot))
            if (Input.GetButton("FirePrimary") || Input.GetAxis("FirePrimary") == 1)
            {
                if (canShoot) // Used for rewind system
                {
                    gunController.shoot();
                    player.setFace(PlayerController.EMOTION.ANGRY);
                    pUI.changeToSad();
                }
            }
            else
            {
                player.setFace(PlayerController.EMOTION.HAPPY);
                pUI.changeToHappy();
            }
        }

        /// <summary>
        /// Checks if the player is trying to switch weapon
        /// </summary>
        private void inputCheck_WeaponSwitch()
        {
            // KB+M
            for (int i = 0; i < weaponSwitches.Count; i++)
            {
                if (Input.GetKeyDown(weaponSwitches[i]))
                {
                    if (weapons.Count > i)
                    {
                        currentWeapon = i;
                    }
                    else
                        Debug.LogError("Weapon not mapped to button " + weaponSwitches[i] + "!");
                }
            }

            // TODO: Controller
            if (Input.GetButtonDown("NextWeapon"))
            {
                if (currentWeapon >= weaponSwitches.Count-1)
                    currentWeapon = 0;
                else
                    currentWeapon++;
            }
            else if (Input.GetButtonDown("PreviousWeapon"))
            {
                if (currentWeapon == 0)
                    currentWeapon = weaponSwitches.Count-1;
                else
                    currentWeapon--;
            }

            gunController.setGun(weapons[currentWeapon]);
            weaponUISwitch.switchWeaponUI(currentWeapon);
        }

        /// <summary>
        /// Checks if the player is trying to use a time mechanic
        /// </summary>
        private void inputCheck_TimeMechanic()
        {
            //if (timeMechanic != null && Input.GetKeyDown(kbmTimeMechanic) || Input.GetKeyDown(controllerTimeMechanic))
            if (timeMechanic != null && Input.GetButtonDown("UseTime"))
                timeMechanic.trigger();
                
        }

        /// <summary>
        /// Checks to see if user tries to dash
        /// </summary>
        private void inputCheck_Dash()
        {
            //if (Input.GetKeyDown(kbmDashMechanic) || Input.GetKeyDown(controllerDashMechanic))
            if (Input.GetButtonDown("Dash"))
                dashMechanic.activate(getDirectionVector());
        }




        /*
           ^
          /|\
         / | \
        /  |  \
           |
           |     o w e r                     \
           o---------------------------------->
                                             /
        */




        /// <summary>
        /// Pauses/unpauses player input
        /// </summary>
        /// <param name="pause">True to pause, False to unpause.</param>
        public void pause(bool pause = true)
        {
            paused = !paused;
            player.getUI().GetComponentInChildren<PauseMenu>().Pause(paused);
        }

        /// <summary>
        /// Returns true if player is paused.
        /// </summary>
        public bool isPaused()
        {
            return paused;
        }

        /// <summary>
        /// Figures out which control method is being used (for turning)
        /// </summary>
        /// <returns>KBM for mouse, CONTROLLER for joystick</returns>
        private ControlMethod getControlMethod()
        {
            if (mouseMoved())
                return ControlMethod.KBM;

            // joystick axis
            if (Input.GetAxis("ShootHorizontal") != 0.0f || Input.GetAxis("ShootVertical") != 0.0f)
                return ControlMethod.CONTROLLER;

            return control;
        }

        /// <summary>
        /// Tests if the mouse has moved since last frame
        /// </summary>
        /// <returns>True if moved, false if still</returns>
        private bool mouseMoved()
        {
            float tolerance = 0.25f;
            Vector2 newPos = getMousePos();
            bool moved = (newPos - mousePos).magnitude > tolerance;
            mousePos = newPos;

            return moved;
        }

        ///
        /// Returns the direction vector of the player
        /// </summary>
        public Vector3 getDirectionVector()
        {
            return directionVector;
        }

        /// <summary>
        /// Prints a statement for each key currently pressed.
        /// 
        /// Used to figure out keycodes without looking them up.
        /// 
        /// Very slow, should only be called when discovering new
        /// buttons to map.
        /// </summary>
        private void printKeys()
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(key))
                    Debug.Log(key);
            }
        }

        /// <summary>
        /// Sets the time mechanic to be toggled with input buttons
        /// </summary>
        /// <param name="timeMechanic">Player's current time mechanic</param>
        public void setTimeMechanic(TimeMechanic.TimeMechanic timeMechanic)
        {
            this.timeMechanic = timeMechanic;
        }
    }
}