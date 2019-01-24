using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class Handgun : AbstractGun
    {
        public void Awake()
        {
            base.init(10, 10, 5, true);
        }
    }
}