﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Gun;

namespace Powerups
{
    public class ShotgunPowerup : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(Shotgun);
        }
    }
}