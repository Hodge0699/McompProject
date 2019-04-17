using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class RandomPowerDrop : MonoBehaviour {

    [System.Serializable]
    public class DropItems
    {
        public string name;
        public GameObject item;
        public int dropRarity; 
    }

    //player health attributes
    [SerializeField]
    private GameObject player;
    [SerializeField]
    PlayerHealthManager playerHealth;
    [SerializeField]
    private float health;
    private GameObject dropHealth;

    public List<DropItems> LootTable = new List<DropItems>();
    public int  ItemDropChance = 50;

    public bool debugging = false;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player(Clone)");
            playerHealth = player.GetComponent<PlayerHealthManager>();
        }
    }


    public void CalculateLoot()
    {
        // Don't drop a power up if enemy not tied to a room - Jake
        if (GetComponent<EnemyType.AbstractEnemy>().getRoom() == null)
            return;

        int Calc_ItemDropChance = Random.Range(0, 101);
        if (Calc_ItemDropChance > ItemDropChance)
        {
            if (debugging)
                Debug.Log("No loot");
            return;
        }

        health = playerHealth.getHealth();
        if (health <= playerHealth.startingHealth / 2)
        {
            Debug.Log("here");
            Calc_ItemDropChance = Random.Range(0, 4);
            if (Calc_ItemDropChance <= 2)
            {
                healthDrop();
            }
        }
        else if (health <= playerHealth.startingHealth / 1.25)
        {
            Debug.Log("getting here");
            Calc_ItemDropChance = Random.Range(0, 4);
            if (Calc_ItemDropChance == 1)
            {
                healthDrop();
            }
        }
        else {
            if (Calc_ItemDropChance <= ItemDropChance)
            {
                int ItemWeight = 0;

                for (int i = 0; i < LootTable.Count; i++)
                {
                    ItemWeight += LootTable[i].dropRarity;
                }

                if (debugging)
                    Debug.Log("ItemWeight= " + ItemWeight);

                int randomValue = Random.Range(0, ItemWeight);

                for (int j = 0; j < LootTable.Count; j++)
                {
                    if (randomValue <= LootTable[j].dropRarity)
                    {
                        GameObject drop = Instantiate(LootTable[j].item, transform.position, Quaternion.identity);

                        // Tie drop to current room - Jake
                        GetComponent<EnemyType.AbstractEnemy>().getRoom().addPowerUpDrop(drop);
                        return;
                    }
                    randomValue -= LootTable[j].dropRarity;

                    if (debugging)
                        Debug.Log("Random Value decreased" + randomValue);
                }
            }
        }
    }

    private void healthDrop()
    {
        for (int i = 0; i < LootTable.Count; i++)
        {
            if (LootTable[i].name == "Health")
            {
                dropHealth = Instantiate(LootTable[i].item, transform.position, Quaternion.identity);
            }
        }
    }
}
