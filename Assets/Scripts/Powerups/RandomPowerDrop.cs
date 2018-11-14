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
    public int  ItemDropChance;

    public void CalculateLoot()
    {
        int Calc_ItemDropChance = Random.Range(0, 101);
        if (Calc_ItemDropChance > ItemDropChance)
        {
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
            Debug.Log("ItemWeight= " + ItemWeight);

            int randomValue = Random.Range(0, ItemWeight);

            for (int j = 0; j < LootTable.Count; j++)
            {
                if (randomValue <= LootTable[j].dropRarity)
                {
                    Instantiate(LootTable[j].item, transform.position, Quaternion.identity);
                    return;
                }
                randomValue -= LootTable[j].dropRarity;
                Debug.Log("Random Value decreased" + randomValue);
            }
        }



    }

   

}
