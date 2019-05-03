using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace effects
{
    public class EffectInstantiator : MonoBehaviour
    {
        /// <summary>
        /// effects class to hold the name, effect and whether or not to use it
        /// </summary>
        [System.Serializable]
        public class Effects
        {
            public string name;
            public GameObject effect;
            public bool use;
        }
        [SerializeField]
        private List<Effects> effectsList = new List<Effects>();

        private void Start()
        {
            foreach(Effects e in effectsList)
            {
                if(e.use == true)
                {
                    Instantiate(e.effect, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
