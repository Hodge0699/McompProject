using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

public class bulletPillar : AbstractEnemy
{

    public float timer = 10.0f;
    private Transform pillarTransform;
    public GunController[] gunController;
    // Use this for initialization
    void Start () {
        pillarTransform = this.GetComponent<Transform>();
        for (int i = 0; i <= 30; i++)
        {
            gunController = new GunController[i];
            
        }
        for(int j = 0; j < 30; j++)
        {
            gunController[j] = GetComponent<GunController>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        timer = timer - Time.deltaTime;
        if(timer < 0)
        {
            //ominDirectionalShooting();
            timer = 10.0f;
            
        }
        
	}
    private void ominDirectionalShooting()
    {
        for (int i = 0; i < 30; i++)
        {
            gunController[i].shoot();
            gunController[i].firePoint.transform.Rotate(Vector3.up, 0.2f);
        }
    }
}
