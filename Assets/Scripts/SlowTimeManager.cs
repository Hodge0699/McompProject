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

    private Player.PlayerInputManager playerInput;

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        playerInput = GetComponent<Player.PlayerInputManager>();

        if (scene.name == "LevelOne")
            gameObject.GetComponent<SlowTimeManager>().enabled = true;
        else
            gameObject.GetComponent<SlowTimeManager>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.isPaused())
            return;

        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0)
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
