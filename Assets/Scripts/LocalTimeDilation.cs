using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTimeDilation : MonoBehaviour {

    private float dilation = 1.0f;

    /// <summary>
    /// Gets the dilation factor that is used for the local delta
    /// </summary>
    /// <returns></returns>
    public float getDilation()
    {
        return dilation;
    }

    /// <summary>
    /// Gets the delta time affected by time dilation
    /// </summary>
    public float getLocalDelta()
    {
        return dilation * Time.deltaTime;
    }


    /// <summary>
    /// Sets the time dilation of this object
    /// </summary>
    /// <param name="dilation">New time dilation</param>
    public void setTimeDilation(float dilation = 1.0f)
    {
        this.dilation = dilation;
    }

    /// <summary>
    /// Modifies the time dilation by set amount
    /// </summary>
    /// <param name="dilation"></param>
    public void modifyTimeDilation(float dilation)
    {
        this.dilation += dilation;
    }
}
