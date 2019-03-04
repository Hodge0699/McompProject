using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBubbleController : MonoBehaviour {

    public Vector3 direction;
    public float speed = 10.0f;
    public float bubbleDuration = 100.0f;
    private float defaultSpeed = 0.0f;
    private float _localTimeScale = 1.0f;
    public List<Rigidbody> r;

    void Start()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }


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
            rb.AddForce(-Physics.gravity + (Physics.gravity * (_localTimeScale * _localTimeScale)), ForceMode.Acceleration);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag != "Untagged"))
        {
            Debug.Log("someone entered time bubble");
            r.Add(other.GetComponent<Rigidbody>());
            speedAdjuster(2.0f, r);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged")
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
