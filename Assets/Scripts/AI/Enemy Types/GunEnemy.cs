using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class GunEnemy : AbstractEnemy
    {
        private GunController gunController;
        private VisionCone pickUpVisionCone;

        protected override void Awake()
        {
            gunController = GetComponentInChildren<GunController>();
            pickUpVisionCone = GetComponents<VisionCone>()[1];

            base.Awake();
        }

        private void Update()
        {
            if (pickUpVisionCone.hasVisibleTargets())
                moveToPickup();
            else if (target == null)
                wander();
            else
            {
                chase();
            }
        }

        private void moveToPickup()
        {
            GameObject target = pickUpVisionCone.getClosestVisibleTarget();

            transform.LookAt(target.transform);
            directionVector = transform.forward;
        }
    }
}