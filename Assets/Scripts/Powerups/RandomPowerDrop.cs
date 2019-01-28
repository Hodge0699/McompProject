using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomPowerDrop : MonoBehaviour {

    [System.Serializable]
    public class DropItems
    {
        public string name;
        public GameObject item;
        public int dropRarity; 
    }

    public List<DropItems> LootTable = new List<DropItems>();
    public int  ItemDropChance = 50;

    private Player.PlayerController player;

    public bool debugging = false;

    private void Awake()
    {
        player = FindObjectOfType<Player.PlayerController>();
    }

    public void CalculateLoot()
    {
        int Calc_ItemDropChance = Random.Range(0, 101);
        if (Calc_ItemDropChance > ItemDropChance)
        {
            if (debugging)
                Debug.Log("No loot");
            return;
        }
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
                    player.getCurrentRoom().addPowerUpDrop(drop);
                    return;
                }
                randomValue -= LootTable[j].dropRarity;

                if (debugging)
                    Debug.Log("Random Value decreased" + randomValue);
            }
        }
    }
}
