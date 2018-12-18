using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Powerups
{
    public class Shotgun : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(Shotgun);
        }
    }
}