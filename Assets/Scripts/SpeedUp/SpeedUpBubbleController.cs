using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBubbleController : MonoBehaviour {

    public Vector3 direction;
    public float speed = 10.0f;
    public float bubbleDuration = 100.0f;

    void Start()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }


    void Update()
    {
        // destroys the bubble after a set time
        bubbleDuration -= Time.deltaTime;

        if(bubbleDuration <= 0)
        {
            Destroy(this.gameObject);
        }
    }//


    private void OnTriggerEnter(Collider other)
    {
        // if something enters the field of the bubble increase its speed
        if (other.gameObject.tag == "SpeedUp")
        {
            //debug
            Debug.Log("I'm registering the player is inside the bubble");
            //other.GetComponent<PlayerController>().moveSpeed = other.GetComponent<PlayerController>().moveSpeed * speed;
            other.transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
