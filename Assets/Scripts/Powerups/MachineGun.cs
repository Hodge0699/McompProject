using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun.Powerups
{
    public class MachineGun : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(Gun.MachineGun);
        }
    }
}