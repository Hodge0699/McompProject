using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBubbleController : MonoBehaviour {

    public Vector3 direction;
    public float speed = 10.0f;
    public float bubbleDuration = 100.0f;
    private float _localTimeScale = 1.0f;
    public List<Rigidbody> r;
    [Header("value to control speed inside bubble")]
    [SerializeField]
    private int speedInside = 5;
    [SerializeField]
    private GameObject player;
    private GameObject enemy;
    Vector3 heading;
    Vector3 enemyDirection;


    void Update()
    {
        // destroys the bubble after a set time
        bubbleDuration -= Time.deltaTime;

        if (bubbleDuration <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        foreach (Rigidbody rb in r)
        {
            // checks if the rigidbody belongs to the player
            if (rb.gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                Vector3 directionVector = rb.gameObject.GetComponent<Player.PlayerInputManager>().getDirectionVector();
                rb.AddForce(directionVector * speedInside, ForceMode.Impulse);
            }
            // else its speeding up the enemies.
            else
            {
                if (enemy != null)
                {
                    enemy = rb.gameObject;
                    heading = enemy.transform.position - player.transform.position;
                    float distance = heading.magnitude;
                    enemyDirection = heading / distance;
                    rb.AddForce(enemyDirection * speedInside, ForceMode.Impulse);
                }
            }
            //rb.AddForce(-Physics.gravity + (Physics.gravity * (400 * 400)));
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag != "Untagged") && !other.isTrigger)
        {
            r.Add(other.GetComponent<Rigidbody>());
            speedAdjuster(2.0f, r);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged" && !other.isTrigger)
        {
            speedAdjuster(1.0f, r);
            r.Remove(other.GetComponent<Rigidbody>());
        }
    }
    /// <summary>
    /// adjusts all the object's rigidbody velocity that is inside the bubble
    /// </summary>
    /// <param name="value"></param>
    /// <param name="other"></param>
    private void speedAdjuster(float value, List<Rigidbody> other)
    {
        float multiplier = value / _localTimeScale;
        foreach (Rigidbody rb in r)
        {
            rb.velocity *= multiplier;
        }
        _localTimeScale = value;
    }
}
