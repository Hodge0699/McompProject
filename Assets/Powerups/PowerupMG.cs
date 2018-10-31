using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMG : MonoBehaviour
{

    float time = 5.0f;
    GunController GC;
    public float BulletSpeed = 0.25f;
    public float Duration = 30f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))

            //StartCoroutine(GC(other));
            Debug.Log("MachineGun Picked Up");
        GC.timeBetweenShots = BulletSpeed;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if(time == 0)
        {
            GC.timeBetweenShots = 0.5f;
        }
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



