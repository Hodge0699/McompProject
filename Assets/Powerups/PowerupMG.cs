using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMG : MonoBehaviour
{

    public float BulletSpeed = 2f;
    public float Duration = 30f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        
            StartCoroutine(GC(other));
        
            
    }
    IEnumerator GC(Collider player)
    {
        GunController GC = player.GetComponent<GunController>();
        GC.timeBetweenShots *= BulletSpeed;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(Duration);
        GC.timeBetweenShots /= BulletSpeed;

        Destroy(gameObject);
    }


}
