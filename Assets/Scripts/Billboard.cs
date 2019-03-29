using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public GameObject cam;
    
    private void Update()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        transform.rotation = cam.transform.rotation;
    }
}
