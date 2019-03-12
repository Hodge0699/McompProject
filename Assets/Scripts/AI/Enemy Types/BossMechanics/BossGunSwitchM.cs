using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;
using EnemyType;

public class BossGunSwitchM : MonoBehaviour {

    HealthManager health;
    EnemyType.Bosses.BossEnemy bEnemy;
    private int gunSwitchChanger = 0;

    private void Awake()
    {
        bEnemy = this.GetComponent<EnemyType.Bosses.BossEnemy>();
        health = GetComponent<HealthManager>();
    }
	
	// Update is called once per frame
	void Update () {
        // checks health to switch guns
        if (health.getHealthPercentage() <= 0.5f && health.getHealthPercentage() > 0.25f && gunSwitchChanger == 0)
        {
            bEnemy.getGunController().setGun(typeof(Shotgun), true);
            gunSwitchChanger = 1;
        }
        else if (health.getHealthPercentage() <= 0.25f && health.getHealth() > 0 && gunSwitchChanger == 1)
        {
            bEnemy.getGunController().setGun(typeof(EXDHandgun), true);
            gunSwitchChanger = 2;
        }
    }
}
