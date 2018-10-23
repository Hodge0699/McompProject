using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    int shootableMask;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    public float range = 100f;
    public GunController gun;

    private void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        { shooting(); }
        if (Input.GetMouseButtonUp(0))
        { gun.isFiring = false; }
        //if (Input.GetButtonDown("Right Mouse"))
        //{
        //    gun.timeMechanic();//
        //}
    }


    void shooting()
    {
        gun.isFiring = true;

    }
}
