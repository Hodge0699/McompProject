using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBubbleController : MonoBehaviour {

    public float bubbleDuration = 100.0f;

    [SerializeField]
    private float timeDilation = 2.0f;

    List<LocalTimeDilation> affectedObjects = new List<LocalTimeDilation>();

    void Update()
    {
        // destroys the bubble after a set time
        bubbleDuration -= Time.deltaTime;

        if (bubbleDuration <= 0)
            Destroy(this.gameObject);
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
