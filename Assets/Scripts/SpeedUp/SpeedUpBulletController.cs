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
    /// <summary>
    /// checks for collisions with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter(Collider collision)
    {
        SpeedUpBubbleController timeBubble = Instantiate(tB, collision.transform.position, collision.transform.rotation);
        Destroy(gameObject);
    }
}
