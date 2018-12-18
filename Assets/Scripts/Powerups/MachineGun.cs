using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Powerups
{
    public class MachineGun : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(MachineGun);
        }
    }
}