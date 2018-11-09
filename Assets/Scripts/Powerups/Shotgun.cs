using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun.Powerups
{
    public class Shotgun : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(Gun.Shotgun);
        }
    }
}