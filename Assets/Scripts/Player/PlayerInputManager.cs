using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Input
{
    /// <summary>
    /// Feeds input into player and its guns
    /// </summary>
    public class PlayerInputManager : MonoBehaviour
    {

        PlayerController player;
        GunController gun;

        public KeyCode controlSchemeToggle = KeyCode.P;

        public KeyCode mouseShoot = KeyCode.Mouse0;
        public KeyCode controllerShoot = KeyCode.Joystick1Button4;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();
            gun = transform.Find("GunPrimary").GetComponent<GunController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(controlSchemeToggle))
            {
                player.useController = !player.useController;
                
            }

            // Movement
            if (player.allowPlayerControl)
            {
                Vector3 dir = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f, UnityEngine.Input.GetAxisRaw("Vertical"));
                player.setDirectionVector(dir);
            }

            if (UnityEngine.Input.GetKey(mouseShoot) || UnityEngine.Input.GetKey(controllerShoot))
            {
                gun.shoot();
                player.setFace(PlayerController.EMOTION.ANGRY);
            }
            else
                player.setFace(PlayerController.EMOTION.HAPPY);
        }
    }
}