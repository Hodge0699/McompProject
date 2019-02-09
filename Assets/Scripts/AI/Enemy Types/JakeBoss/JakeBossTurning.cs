using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class JakeBossTurning : JakeBoss
    {
        protected override void Start()
        {
            base.Start();

            mainPivot.stopPivot(true);

            rightPivot.goToBound(true);
            leftPivot.goToBound(false);
        }

        protected override System.Type decideState()
        {
            // Can't see player at all
            if (!peripheralVisionCone.hasVisibleTargets())
                return typeof(JakeBossWaiting);

            // Facing player
            if (isFacingPoint(peripheralVisionCone.getClosestVisibleTarget().transform.position))
                return typeof(JakeBossAttacking);

            return this.GetType();
        }

        protected override void stateAction()
        {
            turnTo(peripheralVisionCone.getClosestVisibleTarget());

            // Don't shoot player-side gun
            if (isFacingPoint(peripheralVisionCone.getClosestVisibleTarget().transform.position, transform.right, 1.0f)) // Player to the right
                shoot(false, true);
            else
                shoot(true, false);
        }

        /// <summary>
        /// Returns true if this object is facing the point (with tolerance).
        /// </summary>
        /// <param name="point">Point to look for.</param>
        /// <param name="forward">Leave blank to use this enemy's forward, substitute if testing from another direction.</param>
        /// <param name="tolerance">Tolerance from full 1.0 dot product (should be between 0 and 1)</param>
        /// <returns>True if facing position</returns>
        private bool isFacingPoint(Vector3 point, Vector3? forward = null, float tolerance = 0.025f)
        {
            if (!forward.HasValue)
                forward = transform.forward;

            Vector3 dir = (point - transform.position).normalized;
            return (Vector3.Dot(forward.Value, dir) >= 1.0f - tolerance);
        }
    }
}