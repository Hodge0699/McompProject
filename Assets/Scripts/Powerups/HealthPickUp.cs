using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthManager;


namespace Powerups
{
    public class HealthPickUp : AbstractPowerup
    {
        [SerializeField]
        private float health;
        private void Awake()
        {
            addHealth = health;
        }
    }
}
