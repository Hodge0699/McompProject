using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatSelector : MonoBehaviour {

    public List<GameObject> hats;

    private Player.Runway.RunwayModelDarren darren;
    private Text text;

    private int hatNum;

    private void Start()
    {
        hatNum = 0;

        GameObject j = HatManager.getHat();

        HatManager.setHat(hats[hatNum]);

        darren = GameObject.Find("Darren").GetComponent<Player.Runway.RunwayModelDarren>();
        darren.setHat(hats[hatNum]);

        text = transform.Find("Panel").Find("Text").GetComponent<Text>();
        text.text = hats[hatNum].name;
    }

    /// <summary>
    /// Spawns the next hat in the list, repeats around to hat[0] when overflows
    /// </summary>
    public void nextHat()
    {
        if (hatNum >= hats.Count - 1)
            hatNum = 0;
        else
            hatNum++;

        setHat(hatNum);
    }

    /// <summary>
    /// Spawns the previous hat in the list, repeats around to hat[n-1] when less than 0
    /// </summary>
    public void previousHat()
    {
        if (hatNum <= 0)
            hatNum = hats.Count - 1;
        else
            hatNum--;

        setHat(hatNum);
    }

    /// <summary>
    /// Sets a hat from the array into static HatManager, darren and UI textbox
    /// </summary>
    /// <param name="hatNum">Index of hat in array</param>
    private void setHat(int hatNum)
    {
        HatManager.setHat(hats[hatNum]);
        darren.setHat(hats[hatNum]);
        text.text = hats[hatNum].name;
    }
}
