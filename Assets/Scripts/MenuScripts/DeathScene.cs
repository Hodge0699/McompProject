using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour {


    public Button deathButton;
    float timer = 15.0f;

	// Use this for initialization
	void Start () {
        //deathButton = GetComponentInParent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        if (deathButton.IsActive())
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0)
        {
            mainMenu();
        }
	}

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
