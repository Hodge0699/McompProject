using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : HealthManager {

    public override void hurt(float damageAmount)
    {
        base.hurt(damageAmount);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            transform.LookAt(player.transform);
    }
}
