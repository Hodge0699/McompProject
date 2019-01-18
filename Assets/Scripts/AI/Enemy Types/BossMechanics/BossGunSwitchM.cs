using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;
using EnemyType;

public class BossGunSwitchM : MonoBehaviour {

    BossEnemy bEnemy;
    protected System.Type gun;
    private int gunSwitchChanger = 0;

    private void Awake()
    {
        bEnemy = this.GetComponent<BossEnemy>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (bEnemy.currentHealth <= bEnemy.health / 2 && bEnemy.currentHealth >= bEnemy.health / 4 && gunSwitchChanger == 0)
        {
            gun = typeof(Shotgun);
            bEnemy.gunController.setGun(gun, 300);
            gunSwitchChanger = 1;
        }
        else if (bEnemy.currentHealth <= bEnemy.health / 4 && bEnemy.currentHealth > 0 && gunSwitchChanger == 1)
        {
            gun = typeof(EXDHandgun);
            bEnemy.gunController.setGun(gun, 300);
            gunSwitchChanger = 2;
        }
    }
}
