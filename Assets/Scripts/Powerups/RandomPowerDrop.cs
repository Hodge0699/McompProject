using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerDrop : MonoBehaviour {

    public powerupSG1 Shotgun;
    public PowerupMG MachineGun;
    public PowerupsEXD ExtraDamage;
 
	
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
