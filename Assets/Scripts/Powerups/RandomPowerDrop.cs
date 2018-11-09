using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerDrop : MonoBehaviour {

    public Gun.Powerups.Shotgun Shotgun;
    public Gun.Powerups.MachineGun MachineGun;
    public Gun.Powerups.EXDHandgun ExtraDamage;
 
	
    public class ItemDrops
    {
        
    }

    public List<ItemDrops> ItemTable = new List<ItemDrops>();
    public int ItemDropChance;


    public  void DropRate()
    {
       int calc_dropRate = Random.Range(0, 101);
       if(calc_dropRate > ItemDropChance)
        {
            Debug.Log("No powerups");
            return;
        }
      
      
    }

}
