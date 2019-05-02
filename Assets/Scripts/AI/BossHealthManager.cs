using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EnemyType;

//
// Originally written by Nicky, cleaned up by Jake
//
namespace HealthManager
{
    public class BossHealthManager : EnemyHealthManager
    {
        private void Start()
        {
            healthBar = GetComponent<BossUI>().getUI().transform.Find("BossHealth").transform.Find("Bars").transform.Find("Healthbar").GetComponent<Image>();
        }
    }
}