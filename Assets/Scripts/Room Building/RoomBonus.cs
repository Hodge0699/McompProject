﻿
using RoomBuilding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBonus : MonoBehaviour
{

    private Player.PlayerController player;

    public int enemyCount;
    private GameObject enemies;
   
   
    private EnemySpawner enemySpawner;
    // Use this for initialization
    void Start()
    {
        enemyCount = 60;
        enemySpawner = GetComponent<EnemySpawner>();

        GameObject playerObj = Instantiate(Resources.Load("Player")) as GameObject;
        playerObj.transform.position = new Vector3(0, 0.5f, 0);
        playerObj.AddComponent<TimeJump>();
        player = playerObj.GetComponent<Player.PlayerController>();

       
        spawnEnemies();
        //enableEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void enableEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log(enemyCount);
            enemies.transform.GetChild(i).GetComponent<EnemyType.AbstractEnemy>().enabled = true;
        }
    }


    public void addEnemy(EnemyType.AbstractEnemy enemy)
    {
        if (enemies == null)
        {
            enemies = new GameObject();
            enemies.name = "Enemies";
            enemies.transform.parent = this.transform;
        }

       
        enemy.transform.parent = enemies.transform;

    }


    private void spawnEnemies()
    {


        for (int i = 0; i < enemyCount; i++)
        { 



            //GameObject enemy = enemySpawner.spawn(typeof(EnemyType.MeleeEnemy));
            GameObject enemy = enemySpawner.spawn();


            addEnemy(enemy.GetComponent<EnemyType.AbstractEnemy>());
            //Debug.Log(enemyCount);
            enemies.transform.GetChild(i).GetComponent<EnemyType.AbstractEnemy>().enabled = true;
           // enemyCount--;
        } 
    }
    

}