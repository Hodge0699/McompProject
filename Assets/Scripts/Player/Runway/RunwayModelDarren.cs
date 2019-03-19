using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runway
{
    public class RunwayModelDarren : MonoBehaviour
    {
        public float rotationSpeed = 50.0f;

        private GameObject hat;

        private void Update()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        public void setHat(GameObject hatAsset)
        {
            if (hat != null)
                Destroy(hat);

            hat = Instantiate(hatAsset) as GameObject;
            hat.transform.SetParent(transform.Find("Hat Anchor"), false);
            hat.name = "Hat";
        }
    }
}