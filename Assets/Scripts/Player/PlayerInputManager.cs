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
        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;

        private PlayerController player;
        private new Rigidbody rigidbody;
        private GunController gun;

        public KeyCode controlSchemeToggle = KeyCode.P;

        public KeyCode mouseShoot = KeyCode.Mouse0;
        public KeyCode controllerShoot = KeyCode.Joystick1Button4;

        private Plane mousePlane; // Plane to track the mouse position on screen.
        private Vector2 mousePos;

        private bool debugging = false;

        public bool allowInput = true;
        private float forceMoveDistanceCounter = 0.0f;
        private float forceMoveDistanceTarget = 0.0f;

        private Vector3 directionVector;

        private enum ControlMethod { KBM, CONTROLLER };
        private ControlMethod control = ControlMethod.KBM;

        private Vector3 lastMoveDir;

        [Header("Dash Distance")]
        [SerializeField]
        float dashDistance = 30f;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();
            rigidbody = GetComponent<Rigidbody>();
            gun = transform.Find("GunPrimary").GetComponent<GunController>();

            mousePlane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));
        }

        // Update is called once per frame
        void Update()
        {
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
            else
                directionVector = Vector3.zero;
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
            if (UnityEngine.Input.GetKey(mouseShoot) || UnityEngine.Input.GetKey(controllerShoot))
            {
                if (canShoot) // Used for rewind system
                {
                    gun.shoot();
                    player.setFace(PlayerController.EMOTION.ANGRY);
                }
            }
            else
                player.setFace(PlayerController.EMOTION.HAPPY);
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

        private bool CanMove(Vector3 dir, float distance)
        {
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(transform.position, dir, out hitInfo, 10f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            GameObject Gobject = hitInfo.collider.gameObject;
            if (Gobject.tag == "Untagged" && hitInfo.distance >= distance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool TryDash(Vector3 baseMoveDir, float distance)
        {
            Vector3 moveDir = baseMoveDir;
            bool canMove = CanMove(moveDir, distance);
            if (!canMove)
            {
                // can't move diagonally
                moveDir = new Vector3(baseMoveDir.x, 0f).normalized;
                canMove = moveDir.x != 0f && CanMove(moveDir, distance);
                if (!canMove)
                {
                    // can't move horizontally
                    moveDir = new Vector3(0f, baseMoveDir.y).normalized;
                    canMove = moveDir.y != 0f && CanMove(moveDir, distance);
                }
            }

            if (canMove)
            {
                lastMoveDir = moveDir;
                transform.position += moveDir * distance;
                return true;
            }
            else
            {
                return false;
            }

        }
        private void dash()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryDash(lastMoveDir, dashDistance);
                //if (CanMove(lastMoveDir, dashDistance))
                //    transform.position += lastMoveDir * dashDistance;
            }
        }
    }
}