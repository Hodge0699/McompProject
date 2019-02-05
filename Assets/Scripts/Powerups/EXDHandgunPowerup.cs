using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Gun;

namespace Powerups
{
    public class EXDHandgunPowerup : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(EXDHandgun);
            ammo = 15;
        }
    }
}