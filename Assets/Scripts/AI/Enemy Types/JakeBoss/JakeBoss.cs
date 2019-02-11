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
            setState(newState);
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
        protected virtual void onStateSwitch(JakeBoss newState)
        {
            newState.copyBaseVariables(this);
            Destroy(this);
        }

        /// <summary>
        /// Moves onto the next state
        /// </summary>
        /// <param name="state">State to switch to.</param>
        protected void setState(System.Type state)
        {
            // Don't switch if same state
            if (state == this.GetType())
                return;

            gameObject.AddComponent(state);
            onStateSwitch(GetComponents<JakeBoss>()[1]);
        }

        /// <summary>
        /// Copies base AbstractEnemy variables into a new behaviour
        /// </summary>
        /// <param name="enemy">Behaviour to copy into</param>
        public void copyBaseVariables(AbstractEnemy enemy)
        {
            this.movementSpeed = enemy.movementSpeed;
            this.maxDistance = enemy.maxDistance;
            this.turnSpeed = enemy.turnSpeed;
            this.myRoom = enemy.getRoom();
        }

        public override void onDeath()
        {
            GameObject sceneManager = GameObject.Find("SceneManager");

            if (sceneManager != null)
                sceneManager.GetComponent<sceneTransitions.SceneTransitions>().LoadNextScene();
        }
    }
}