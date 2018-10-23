using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBulletController : MonoBehaviour
{
    public float speed = 5.0f;
    public SpeedUpBubbleController tB;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(gameObject, 1);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("I'm getting inside the collision function when the bullet hits a viable target" + " " + collision.gameObject.tag);
        if (collision.gameObject.tag == "SpeedUp")
        {
            Debug.Log("I'm getting inside the collision function when the bullet hits a viable target");
            Destroy(gameObject);

            SpeedUpBubbleController timeBubble = Instantiate(tB, collision.transform.position, collision.transform.rotation);
        }
    }
}
