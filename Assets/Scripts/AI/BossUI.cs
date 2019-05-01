using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossUI : MonoBehaviour {
    [SerializeField]
    private GameObject bossUI;

    [Header("BossUI Information")]
    [SerializeField]
    private string bossName;
    private GameObject nameGO;
    // Use this for initialization
    private void Awake()
    {
        bossUI = Instantiate(Resources.Load("BossUI")) as GameObject;
        bossUI.transform.parent = this.transform;
        nameGO = bossUI.transform.Find("BossHealth").transform.Find("Name").gameObject;
        nameGO.GetComponent<Text>().text = bossName;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
