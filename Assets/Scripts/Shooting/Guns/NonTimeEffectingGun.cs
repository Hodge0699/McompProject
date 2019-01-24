using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class NonTimeEffectingGun : AbstractGun
    {
        public void Awake()
        {
            init(10, 10, 5, false);
        }
    }
}
