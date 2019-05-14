using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    // I should really make polymorphic state and state
    // machine classes at this point but its May :/

    public abstract class FinalBoss : AbstractEnemy 
    {
        protected GameObject player; // This boss will know where the player is constantly
        protected Weapon.SawBlade sawBlade;
        protected MeshRenderer meshRenderer;

        protected HealthManager.HealthManager healthManager;

        // Use this for initialization
        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            sawBlade = GetComponentInChildren<Weapon.SawBlade>();
            meshRenderer = GetComponent<MeshRenderer>();

            healthManager = GetComponent<HealthManager.HealthManager>();
        }

        // Update is called once per frame
        void Update()
        {
            System.Type newState = decideState();
            switchToBehaviour(newState);
            stateAction();
        }

        /// <summary>
        /// Carries out all actions of the state
        /// </summary>
        protected abstract void stateAction();

        /// <summary>
        /// Decides which state to switch to after this frame
        /// </summary>
        protected abstract System.Type decideState();

        public override void onDeath()
        {
            GameObject sceneManager = GameObject.Find("SceneManager");

            if (sceneManager != null)
                sceneManager.GetComponent<sceneTransitions.SceneTransitions>().LoadNextScene();
        }
    }
}