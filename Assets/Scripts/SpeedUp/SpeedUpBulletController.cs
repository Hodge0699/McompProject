using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBulletController : BulletController
{
    /// <summary>
    /// checks for collisions with the bullet
    /// </summary>
    /// <param name="collision"></param>
    override protected void OnCollisionEnter(Collision collision)
    {
        if (!ignoreTags.Contains(collision.gameObject.tag))
            Instantiate(Resources.Load("TimeBubble"), collision.transform.position, collision.transform.rotation, GameObject.Find("Active Bullets").transform);

        Destroy(gameObject);
    }
}
