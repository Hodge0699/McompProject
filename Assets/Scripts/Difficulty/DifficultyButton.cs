using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour {

    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();

        writeButton();
	}
	
    /// <summary>
    /// Steps through the difficulty and writes it to the button
    /// </summary>
    public void buttonClicked()
    {
        Difficulty.stepDifficulty();
        writeButton();
    }

    /// <summary>
    /// Writes the current difficulty to the button correctly formatted.
    /// </summary>
    public void writeButton()
    {
        text.text = "Difficulty: " + Difficulty.getDifficultyString();
    }
}
