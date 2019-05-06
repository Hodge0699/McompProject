using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType.Bosses;
using HealthManager;

public class wallDamage : MonoBehaviour {

    [SerializeField]
    private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyType.Bosses.FinalBossCharging>() != null)
        {
            other.GetComponent<HealthManager.HealthManager>().hurt(damage, true);
        }
    }
}
