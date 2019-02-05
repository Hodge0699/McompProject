using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Gun;

namespace Powerups
{
    public class MachineGunPowerup : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(MachineGun);
            ammo = 60;
        }
    }
}