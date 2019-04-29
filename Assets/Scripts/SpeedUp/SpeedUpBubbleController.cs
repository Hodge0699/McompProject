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
    [SerializeField]
    private GameObject enemy;
    Vector3 heading;
    Vector3 enemyDirection;


    void Update()
    {
        // destroys the bubble after a set time
        bubbleDuration -= Time.deltaTime;
        if(player == null)
        {
            player = GameObject.Find("Player(Clone)");
        }
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
            if (rb.gameObject == player)
            {
                Vector3 directionVector = rb.gameObject.GetComponent<Player.PlayerInputManager>().getDirectionVector();
                rb.AddForce(directionVector * speedInside, ForceMode.Impulse);
            }
            // else its speeding up the enemies.
            else
            {
                enemy = rb.gameObject;
                rb.AddForce(enemy.transform.forward * speedInside, ForceMode.Impulse);

            }
        }


    }
    // find all objects who enter the time bubble
    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag != "Untagged") && !other.isTrigger)
        {
            r.Add(other.GetComponent<Rigidbody>());
        }
    }
    // remove any objects who leaves the time bubble
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged" && !other.isTrigger)
        {
            r.Remove(other.GetComponent<Rigidbody>());
        }
    }

}
