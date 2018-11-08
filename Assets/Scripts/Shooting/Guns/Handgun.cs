using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
{
    public class Handgun : AbstractGun
    {
        public void Awake()
        {
            base.init(10, 10, 1);
        }
    }
}