using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour
{

    [SerializeField]
    bool isSlowMotion = false;

    public float slowDownFactor = 0.05f;
    private float normTimeFactor = 1.0f;

    //public float slowDownLength = 2f;

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
