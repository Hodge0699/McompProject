using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimeMechanic
{
    public class TimeMechanic : MonoBehaviour
    {
        [SerializeField]
        private string level = "";

        protected virtual void Start()
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name != level)
            {
                enabled = false;
                return;
            }

            GetComponent<Player.PlayerInputManager>().setTimeMechanic(this);
        }

        /// <summary>
        /// Called when PlayerInputManager recieves input to use TimeMechanic
        /// </summary>
        public virtual void trigger() { }
    }
}