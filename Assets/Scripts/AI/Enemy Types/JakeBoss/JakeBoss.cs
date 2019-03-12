using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public abstract class JakeBoss : AbstractEnemy
    {
        protected GunPivot mainPivot;
        protected GunPivot rightPivot;
        protected GunPivot leftPivot;

        protected GunController gunRight;
        protected GunController gunLeft;

        protected VisionCone peripheralVisionCone;

        // Use this for initialization
        protected virtual void Start()
        {
            // Pivots
            mainPivot = transform.Find("GunPivot").GetComponent<GunPivot>();
            rightPivot = mainPivot.transform.Find("GunRightPivot").GetComponent<GunPivot>();
            leftPivot = mainPivot.transform.Find("GunLeftPivot").GetComponent<GunPivot>();


            // Guns
            gunRight = rightPivot.transform.Find("GunRight").GetComponent<GunController>();
            gunLeft = leftPivot.transform.Find("GunLeft").GetComponent<GunController>();

            // Peripheral vision cone
            peripheralVisionCone = GetComponents<VisionCone>()[1];
        }

        // Update is called once per frame
        void Update()
        {
            System.Type newState = decideState();
            switchToBehaviour(newState);
            stateAction();
        }

        /// <summary>
        /// Shoots guns
        /// </summary>
        /// <param name="shootRight">Should right gun be fired?</param>
        /// <param name="shootLeft">Should left gun be fired?</param>
        protected virtual void shoot(bool shootRight = true, bool shootLeft = true)
        {
            if (shootRight)
                gunRight.shoot();

            if (shootLeft)
                gunLeft.shoot();
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
        /// Copies variables into new state and destroys gameobject.
        /// 
        /// Should be overrided by any behaviours that alter pivots to reset them
        /// back to stopped at their centre.
        /// </summary>
        /// <param name="newState">Next state that is being switched to.</param>
        protected abstract void onStateSwitch(JakeBoss newState);

        /// <summary>
        /// Switches enemy behaviour
        /// </summary>
        /// <param name="state">Behaviour to switch to</param>
        protected override bool switchToBehaviour(Type behaviour, bool destroyOldBehaviour = false, bool copyVariables = true)
        {
            if (!base.switchToBehaviour(behaviour, destroyOldBehaviour, copyVariables))
                return false; // Couldn't switch, don't call onStateSwitch

            onStateSwitch(GetComponents<JakeBoss>()[1]);

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