using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour {

    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;

	// Update is called once per frame
	void Update () {
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

    public void SlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
    }
}
