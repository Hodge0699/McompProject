using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Powerups
{
    public class EXDHandgun : AbstractPowerup
    {
        private void Awake()
        {
            gun = typeof(EXDHandgun);
        }
    }
}