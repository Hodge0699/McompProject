using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeJump : MonoBehaviour
{

    //Light sceneLight;
    GameObject sceneLight;
    GameObject sceneCamera;

    private float Jumpdistance = 200;
    private bool teleported;


    // Use this for initialization
    void Start()
    {
        //sceneLight = FindObjectOfType<Light>();
        sceneLight = GameObject.Find("Directional Light");
        sceneCamera = GameObject.Find("Main Camera(Clone)");
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log(sceneLight);
            Debug.Log(sceneCamera);
            teleported = !teleported;

            //position of the object that will be moved (it is only player for now)
            Vector3 pos = gameObject.transform.position;

            if (teleported == true) FutureJump(pos);
            else PresentJump(pos);
          
        }
    }


    void FutureJump(Vector3 pos)
    {


       // sceneCamera.GetComponent<CameraController>().BlurTransition();
        sceneLight.SetActive(false);
        gameObject.transform.position = new Vector3(pos.x + Jumpdistance, pos.y , pos.z);
        sceneCamera.transform.position = new Vector3(pos.x + Jumpdistance, pos.y , pos.z);

    }

    void PresentJump(Vector3 pos)
    {
        sceneLight.SetActive(true);
        gameObject.transform.position = new Vector3(pos.x - Jumpdistance, pos.y, pos.z);
        sceneCamera.transform.position = new Vector3(pos.x - Jumpdistance, pos.y , pos.z);
    }


}
