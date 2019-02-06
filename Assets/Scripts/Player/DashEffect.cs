using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        destroyDash();
	}
    public void destroyDash()
    {
        Destroy(gameObject, 0.5f);
    }
}
