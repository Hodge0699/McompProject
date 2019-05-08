using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace effects
{
    public class EffectController : MonoBehaviour
    {
        private float duration = 999;
        /// <summary>
        /// virtual function to destroy effect object, by default will last until main object is destroyed
        /// </summary>
        /// <param name="duration"></param>
        protected virtual void destroyObject()
        {
            Destroy(gameObject, duration);
        }
        /// <summary>
        /// sets the duration of how long the effect lasts before being destroyed
        /// </summary>
        /// <param name="time"></param>
        protected void setDuration(float time)
        {
            this.duration = time;
        }
        /// <summary>
        /// attachs the gameobject to the object that instanties it
        /// </summary>
        /// <param name="parent"></param>
        protected void attachToParent(GameObject parent)
        {
            //parent.transform.SetParent(this.transform);
            this.transform.SetParent(parent.transform);
        }
    }
}
