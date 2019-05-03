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

        // Use this for initialization
        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            sawBlade = GetComponentInChildren<Weapon.SawBlade>();
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

        /// <summary>
        /// Can be used as a destructor for state or to copy variables into new state.
        /// </summary>
        /// <param name="newState">Next state that is being switched to.</param>
        protected virtual void onStateSwitch(FinalBoss newState) { }

        /// <summary>
        /// Switches enemy behaviour
        /// </summary>
        /// <param name="state">Behaviour to switch to</param>
        protected override bool switchToBehaviour(Type behaviour, bool destroyOldBehaviour = false, bool copyVariables = true)
        {
            if (!base.switchToBehaviour(behaviour, destroyOldBehaviour, copyVariables))
                return false; // Couldn't switch, don't call onStateSwitch

            onStateSwitch(GetComponents<FinalBoss>()[1]);

            Destroy(this);

            return true;
        }

        public override void onDeath()
        {
            GameObject sceneManager = GameObject.Find("SceneManager");

            if (sceneManager != null)
                sceneManager.GetComponent<sceneTransitions.SceneTransitions>().LoadNextScene();
        }
    }
}