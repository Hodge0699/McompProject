using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

public class BossTeleportM : MonoBehaviour {

    private List<GameObject> teleportLocations = new List<GameObject>();
    private BossEnemy bEnemy;

    private void Start()
    {
        bEnemy = this.GetComponent<BossEnemy>();

        // Code I told Nicky to type while making nuggets that ended up not working because he had that ^ line
        // below it and it was called in Awake instead of Start. So I ended up doing it on my branch and 
        // fixing it but he wanted a mention anyway - Jake.
        Transform teleContainer = bEnemy.getRoom().transform.Find("BossTeleporter").transform;
        for (int i = 0; i < teleContainer.childCount; i++)
            teleportLocations.Add(teleContainer.GetChild(i).gameObject);
    }
}
