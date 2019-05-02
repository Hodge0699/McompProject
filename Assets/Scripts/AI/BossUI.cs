using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossUI : MonoBehaviour {
    private GameObject bossUI;

    private string bossName;

    // Use this for initialization
    private void Awake()
    {
        bossUI = Instantiate(Resources.Load("BossUI")) as GameObject;
        bossUI.transform.SetParent(this.transform);

        GameObject nameGO = bossUI.transform.Find("BossHealth").transform.Find("Name").gameObject;
        nameGO.GetComponent<Text>().text = bossName;
    }

    /// <summary>
    /// Returns the UI GameObject
    /// </summary>
    public GameObject getUI()
    {
        return bossUI;
    }
}
