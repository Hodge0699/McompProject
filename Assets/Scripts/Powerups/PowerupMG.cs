using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMG : MonoBehaviour
{

    public float time = 5.0f;
    public GunController GC;
    public float BulletSpeed = 0.25f;
    public float Duration = 30f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StartCoroutine(GC(other));
            Debug.Log("MachineGun Picked Up");
            GC = other.GetComponentInChildren<GunController>();
            
            //GC.timeBetweenShots = 0.25f;
            GC.setGun(GC.gameObject.AddComponent<Gun.MachineGun>());
            GC.resetTime();
        }
        
    }

    private void Reset()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Debug.Log("getting inside the reset function");
            GC.currentGun = new Gun.Handgun();
        }
        time = 5.0f;
    }
}    
    //IEnumerator GC(Collider player)
    //{
    //    GunController GC = player.GetComponent<GunController>();
    //    GC.timeBetweenShots = BulletSpeed;
   
    //    GetComponent<MeshRenderer>().enabled = false;
    //    GetComponent<Collider>().enabled = false;
        
    //    yield return new WaitForSeconds(Duration);
    //    GC.timeBetweenShots = 0.5f;


    //    Destroy(gameObject);
    //}



