using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeJump : MonoBehaviour
{


    private float distance = 100;
    private bool teleported;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            teleported = !teleported;

            Vector3 pos = gameObject.transform.position;

            if (teleported == true)
            {

                gameObject.transform.position = new Vector3(pos.x + distance, pos.y, pos.z);
            }
            else
            {

                gameObject.transform.position = new Vector3(pos.x - distance, pos.y, pos.z);
            }


        }
    }
}
