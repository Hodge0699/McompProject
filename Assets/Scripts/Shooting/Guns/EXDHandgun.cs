﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class EXDHandgun : AbstractGun
    {
        public float damageMultiplyer = 2.0f;

        public void Awake()
        {
            // Temporarily instantiate base handgun to get variables
            Handgun baseHandgun = gameObject.AddComponent<Handgun>();

            base.init(baseHandgun.damage * damageMultiplyer, baseHandgun.speed, baseHandgun.fireRate * 0.75f, 30);

            Destroy(baseHandgun);
        }
    }
}