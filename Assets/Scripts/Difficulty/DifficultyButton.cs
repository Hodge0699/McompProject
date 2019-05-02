using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyButton : MonoBehaviour {

    //private Text text;
    private TextMeshProUGUI tmp;

	// Use this for initialization
	void Start () {
        tmp = GetComponentInChildren<TextMeshProUGUI>();

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
        tmp.SetText(Difficulty.getDifficultyString());
    }
}
