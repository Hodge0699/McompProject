using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 12.0f;
    public float lifespan = 1.0f;

    private void Awake()
    {
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
