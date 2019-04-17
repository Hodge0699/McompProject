using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyHealthUI : MonoBehaviour {

    private GameObject player;
    private GameObject BossUI;
	// Update is called once per frame
	void Update () {
		if(player == null)
        {
            player = GameObject.Find("Player");
            BossUI = player.transform.Find("BossHealth").gameObject;
            BossUI.SetActive(true);
        }
	}
}
