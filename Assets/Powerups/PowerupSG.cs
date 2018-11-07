using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSG : MonoBehaviour {
    public float time = 5.0f;
    public GunController GC;
    public float BulletSpeed = 0.25f;
    public float Duration = 30f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
         Debug.Log("ShotGun Pickup");   
        }
           
    }

    private void Reset()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
           
           
        }
        time = 5.0f;
    }
}
