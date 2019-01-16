using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour
{

    [SerializeField]
    bool isSlowMotion = false;

    public float slowDownFactor = 0.05f;
    private float normTimeFactor = 1.0f;

    //public float slowDownLength = 2f;

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "LevelOne")
            gameObject.GetComponent<SlowTimeManager>().enabled = true;
        else
        {
            gameObject.GetComponent<SlowTimeManager>().enabled = false;
            StopSlowMotion();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<PlayerController>().movement.magnitude <= 0)
            StartSlowMotion();
        else
            StopSlowMotion();
    }

    void StartSlowMotion()
    {
        isSlowMotion = true;

        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void StopSlowMotion()
    {
        isSlowMotion = false;
        Time.timeScale = normTimeFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
