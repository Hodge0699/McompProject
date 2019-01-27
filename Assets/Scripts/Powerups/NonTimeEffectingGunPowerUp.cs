using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Gun;

namespace Powerups
{
    public class NonTimeEffectingGunPowerUp : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(NonTimeEffectingGun);
        }
    }
}
