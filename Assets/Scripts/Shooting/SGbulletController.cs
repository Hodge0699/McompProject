using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGbulletController : MonoBehaviour
{
    public float speed = 12.0f;
    public float lifespan = 1.0f;

    private void Awake()
    {
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
