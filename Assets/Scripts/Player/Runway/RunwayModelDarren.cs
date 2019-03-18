using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runway
{
    public class RunwayModelDarren : MonoBehaviour
    {
        public GameObject hat;
        public float rotationSpeed = 50.0f;

        private void Start()
        {
            GameObject h = Instantiate(hat);
            h.transform.SetParent(transform.Find("Hat Anchor"), false);
            h.name = "Hat";
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }
}