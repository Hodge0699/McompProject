using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;
using EnemyType;

public class BossGunSwitchM : MonoBehaviour {

    HealthManager health;
    BossEnemy bEnemy;
    private int gunSwitchChanger = 0;

    private void Awake()
    {
        bEnemy = this.GetComponent<BossEnemy>();
        health = GetComponent<HealthManager>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // checks health to switch guns
        if (health.getHealthPercentage() <= 0.5f && health.getHealthPercentage() > 0.25f && gunSwitchChanger == 0)
        {
            bEnemy.gunController.setGun(typeof(Shotgun));
            bEnemy.gunController.getGun().giveUnlimitedAmmo();
            gunSwitchChanger = 1;
        }
        else if (health.getHealthPercentage() <= 0.25f && health.getHealth() > 0 && gunSwitchChanger == 1)
        {
            bEnemy.gunController.setGun(typeof(EXDHandgun));
            bEnemy.gunController.getGun().giveUnlimitedAmmo();
            gunSwitchChanger = 2;
        }
    }
}
