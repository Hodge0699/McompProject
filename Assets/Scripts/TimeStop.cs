using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimeStop : MonoBehaviour
{


    [SerializeField]

    //uncomment onces levelThree has been created

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "LevelThree")
        {
            this.enabled = true;
            GetComponent<LocalTimeDilation>().unscaled = true;
        }
        else
            this.enabled = false;

    }

    void Update()
    {



        if (Input.GetKeyDown("e"))
        {
            if (Time.timeScale == 1.0)
                Time.timeScale = 0.0f;

            else
                Time.timeScale = 1.0f;
        }             
    }
}