using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{

    [SerializeField]

    bool pausetime = false;

    public float PauseTime = 0.0f;
    private float NonPausedTime = 1.0f;
    
     void Start()
    {
       
    }


    public void Update()
    {
       if(Input.GetKeyDown("space"))
        {
       
            StartPaused(); 
        }
       else
        if(Input.GetKeyUp("space"))
        {
            EndPaused();
        }
    }


    void EndPaused()
    {
        Time.timeScale = NonPausedTime;
    }
    void StartPaused()
    {
        
        pausetime = true;
        Time.timeScale = PauseTime;
        
    }
}
