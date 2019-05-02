using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {

    public int index;
    [SerializeField] bool keydown;
    [SerializeField] int maxIndex;



    // Use this for initialization
    void Start () {
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keydown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                        index++;
                    else
                        index = 0;
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                        index--;
                    else
                        index = maxIndex;
                }
            }
            keydown = true;
        }else
            keydown = false;
	}
}
