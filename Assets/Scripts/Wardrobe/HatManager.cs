using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HatManager {

    static GameObject hat;

    static HatManager()
    {
        hat = Resources.Load("Models\\Hats\\Western Hat\\Western Hat") as GameObject;
    }

    public static GameObject getHat()
    {
        return hat;
    }

    static public void setHat(GameObject newHat)
    {
        hat = newHat;
    }
}
