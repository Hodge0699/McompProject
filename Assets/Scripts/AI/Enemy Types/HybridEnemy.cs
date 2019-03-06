using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class HybridEnemy : MonoBehaviour {

        private GunController gunController;

        private AbstractEnemy behaviourScript;
        private enum Behaviour { MELEE, GUN };
        private Behaviour currentBehaviour;

        // Use this for initialization
        void Start() {
            gunController = GetComponentInChildren<GunController>();
            gunController.getGun().setAmmo(0);

            switchBehaviour(Behaviour.MELEE, true);
        }

        // Update is called once per frame
        void Update() {
            if (currentBehaviour == Behaviour.MELEE)
            {
                if (gunController.hasAmmo(true))
                    switchBehaviour(Behaviour.GUN);
            }
            else if (currentBehaviour == Behaviour.GUN)
            {
                if (!gunController.hasAmmo(true))
                    switchBehaviour(Behaviour.MELEE);
            }
        }

        /// <summary>
        /// Switches behaviours
        /// </summary>
        /// <param name="behaviour">New behaviour to switch to</param>
        /// <param name="force">True to force behaviour switch without checking current behaviour</param>
        void switchBehaviour(Behaviour behaviour, bool force = false)
        {
            if (!force && currentBehaviour == behaviour)
                return;

            Destroy(behaviourScript);

            switch (behaviour)
            {
                case Behaviour.MELEE:
                    behaviourScript = gameObject.AddComponent<MeleeEnemy>();
                    gameObject.GetComponent<MeleeEnemy>().usePickups = true;
                    currentBehaviour = Behaviour.MELEE;
                    break;

                case Behaviour.GUN:
                    behaviourScript = gameObject.AddComponent<GunEnemy>();
                    gunController.switchToBest();
                    currentBehaviour = Behaviour.GUN;
                    break;

                default:
                    break;
            }

        }
    }
}