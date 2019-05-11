using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreTimeBullet : BulletController {

    protected override void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.unscaledDeltaTime);

        lifetime += Time.unscaledDeltaTime;
    }
}
