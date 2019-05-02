using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class Handgun : AbstractGun
    {
        public void Awake()
        {
            base.init(10, 10, 2.5f); // Infinite ammo
        }
    }
}