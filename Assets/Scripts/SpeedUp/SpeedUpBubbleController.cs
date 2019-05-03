using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeMechanic;

public class SpeedUpBubbleController : MonoBehaviour {

    public float bubbleDuration = 100.0f;

    [SerializeField]
    private float timeDilation = 2.0f;
    [SerializeField]
    speedUp pSpeedUp;
    List<LocalTimeDilation> affectedObjects = new List<LocalTimeDilation>();

    void Update()
    {
        // destroys the bubble after a set time
        bubbleDuration -= Time.deltaTime;
        if(pSpeedUp == null)
            pSpeedUp = GameObject.FindObjectOfType<PlayerController>().GetComponent<speedUp>();
        if (bubbleDuration <= 0)
        {
            pSpeedUp.canShoot = true;
            Destroy(this.gameObject);
        }
    }

    // find all objects who enter the time bubble
    private void OnTriggerEnter(Collider other)
    {
        LocalTimeDilation otherObj = other.GetComponent<LocalTimeDilation>();

        if (!other.isTrigger && otherObj != null)
        {
            otherObj.modifyDilation(timeDilation);
            affectedObjects.Add(otherObj);
        }
    }

    // remove any objects who leaves the time bubble
    private void OnTriggerExit(Collider other)
    {
        LocalTimeDilation otherObj = other.GetComponent<LocalTimeDilation>();

        if (otherObj != null && affectedObjects.Contains(otherObj))
        {
            otherObj.modifyDilation(-timeDilation);
            affectedObjects.Remove(otherObj);
        }
    }

    private void OnDestroy()
    {
        foreach (LocalTimeDilation t in affectedObjects)
            t.modifyDilation(-timeDilation);
    }
}
