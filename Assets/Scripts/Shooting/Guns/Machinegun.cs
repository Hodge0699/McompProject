using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
{
    public class MachineGun : AbstractGun
    {
        public void Awake()
        {
            init(10.0f, 10.0f, 15.0f);
        }
    }
}