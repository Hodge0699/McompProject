﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeMechanic
{
    public class LocalTimeDilation : MonoBehaviour
    {

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
        public float getDelta()
        {
            return dilation * Time.deltaTime;
        }


        /// <summary>
        /// Sets the time dilation of this object
        /// </summary>
        /// <param name="dilation">New time dilation</param>
        public void setDilation(float dilation = 1.0f)
        {
            this.dilation = dilation;
        }

        /// <summary>
        /// Modifies the dilation by a set amount
        /// </summary>
        /// <param name="dilation">Amount to modify by</param>
        public void modifyDilation(float dilation)
        {
            this.dilation += dilation;
        }
    }
}