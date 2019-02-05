using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;

    private Player.PlayerInputManager playerInput;

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.PlayerInputManager>();
    }

    public void Pause(bool pause = true)
    {
        pauseMenuUI.SetActive(pause);

        if (pause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        // If input has come from menu rather than player input, we need to send a signal to it
        if (playerInput.isPaused() != pause)
            playerInput.pause(pause);
    }
}
