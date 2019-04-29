using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBubbleController : MonoBehaviour {

    public float bubbleDuration = 100.0f;

    [SerializeField]
    private float timeDilation = 2.0f;

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
        if (!other.isTrigger && other.GetComponent<LocalTimeDilation>() != null)
            other.GetComponent<LocalTimeDilation>().modifyTimeDilation(timeDilation);
    }

    // remove any objects who leaves the time bubble
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.GetComponent<LocalTimeDilation>() != null)
            other.GetComponent<LocalTimeDilation>().modifyTimeDilation(-timeDilation);
    }

}
