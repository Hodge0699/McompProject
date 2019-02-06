using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Feeds input into player and its guns
    /// </summary>
    public class PlayerInputManager : MonoBehaviour
    {
        public bool testInput = false; // Used to figure out keycodes without looking them up,
                                       // should be true only when discovering buttons to map.

        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;


        [Header("Keyboard + Mouse Controls")]
        public KeyCode kbmShoot = KeyCode.Mouse0;
        public KeyCode kbmPause = KeyCode.Escape;

        public List<KeyCode> weaponSwitches = new List<KeyCode>();
        public List<System.Type> weapons = new List<System.Type>();


        [Header("Joystick Controls")]
        public KeyCode controllerShoot = KeyCode.Joystick1Button5;
        public KeyCode controllerPause = KeyCode.Joystick1Button7;


        private Plane mousePlane; // Plane to track the mouse position on screen.
        private Vector2 mousePos;

        private PlayerController player;
        private new Rigidbody rigidbody;
        private GunController gunController;

        private bool debugging = false;

        private bool allowInput = true;
        private float forceMoveDistanceCounter = 0.0f;
        private float forceMoveDistanceTarget = 0.0f;

        private Vector3 directionVector;

        private enum ControlMethod { KBM, CONTROLLER };
        private ControlMethod control = ControlMethod.KBM;

        private bool paused = false; // Is player input halted?

        private Vector3 lastMoveDir;

        [Header("Dash Distance")]
        [SerializeField]
        float dashDistance = 30f;

        [Header("Dash Particle Effect")]
        [SerializeField]
        private GameObject dashEffect;
        // Use this for initialization

        void Start()
        {
            player = GetComponent<PlayerController>();
            rigidbody = GetComponent<Rigidbody>();
            gunController = transform.Find("GunPrimary").GetComponent<GunController>();

            mousePlane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));

            initWeaponTypes();
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


            handlePauseToggle();

            if (paused)
                return;

            control = getControlMethod();

            if (debugging)
                Debug.Log("Control method: " + control);

            move();
            turn(control);
            actions();
        }
        private void FixedUpdate()
        {
            dash();
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
                directionVector = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f, UnityEngine.Input.GetAxisRaw("Vertical"));

            Vector3 movement = directionVector.normalized * player.moveSpeed * Time.deltaTime;
            rigidbody.transform.Translate(movement, Space.World);
            rigidbody.velocity = movement;
            lastMoveDir = movement;

            if (!allowInput)
            {
                forceMoveDistanceCounter += movement.magnitude;

                if (forceMoveDistanceCounter >= forceMoveDistanceTarget)
                {
                    forceMoveDistanceCounter = 0.0f;
                    forceMoveDistanceTarget = 0.0f;
                    allowInput = true;

                    GetComponent<PlayerHealthManager>().setGodmode(true, 1.5f);
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

            GetComponent<PlayerHealthManager>().setGodmode(true);
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

        /// <summary>
        /// Responsible for carrying out all other input actions
        /// </summary>
        private void actions()
        {
            // Shooting
            if (Input.GetKey(kbmShoot) || Input.GetKey(controllerShoot))
            {
                if (canShoot) // Used for rewind system
                {
                    gunController.shoot();
                    player.setFace(PlayerController.EMOTION.ANGRY);
                }
            }
            else
                player.setFace(PlayerController.EMOTION.HAPPY);

            for (int i = 0; i < weaponSwitches.Count; i++)
            {
                if (Input.GetKeyDown(weaponSwitches[i]))
                {
                    if (weapons.Count > i)
                        gunController.setGun(weapons[i]);
                    else
                        Debug.LogError("Weapon not mapped to button " + weaponSwitches[i] + "!");
                }
            }
        }

        /// <summary>
        /// Used only to pause/unpause the game. 
        /// 
        /// Allows input checking even when game is paused.
        /// </summary>
        private void handlePauseToggle()
        {
            if (Input.GetKeyDown(kbmPause) || Input.GetKeyDown(controllerPause))
                pause();
        }

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

        /// <summary>
        /// Checks to see if the player can dash forward without hitting an object
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private void CanMove(Vector3 dir, float distance)
        {
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(transform.position, dir, out hitInfo, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            GameObject Gobject = hitInfo.collider.gameObject;
            if (Gobject.tag == "Untagged" && hitInfo.distance <= 1)
            {

            }
            else if (Gobject.tag == "Untagged" && hitInfo.distance <= 4.0f)
            {
                dashParticle();
                transform.position += dir * (distance - hitInfo.distance) /2;
            }
            else
            {
                dashParticle();
                transform.position += dir * distance;
            }
        }

        private void dashParticle()
        {
            GameObject dash;
            dash = Instantiate(dashEffect, transform.position, Quaternion.identity) as GameObject;
            dash.transform.parent = this.transform;
        }
        /// <summary>
        /// Checks to see if user tries to dash
        /// </summary>
        private void dash()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CanMove(lastMoveDir, dashDistance);
            }
        }

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
    }
}