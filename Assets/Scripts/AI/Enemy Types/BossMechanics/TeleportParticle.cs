using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportParticle : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        destroyTeleport();
    }
    public void destroyTeleport()
    {
        Destroy(gameObject, 1.5f);
    }
}
