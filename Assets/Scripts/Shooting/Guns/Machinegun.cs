using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class MachineGun : AbstractGun
    {
        public void Awake()
        {
            init(10.0f, 10.0f, 10.0f, 300);
        }
    }
}