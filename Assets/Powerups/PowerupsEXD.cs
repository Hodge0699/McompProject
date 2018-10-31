using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsEXD : MonoBehaviour
{
    public float multiplier = 2f;
    public float duration = 4.0f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))

            StartCoroutine(PC(other));
        Debug.Log("Power Up Picked Up");
    }
    IEnumerator PC(Collider player)
    {
        PlayerController PC = player.GetComponent<PlayerController>();
        PC.Damage *= multiplier;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        
        yield return new WaitForSeconds(duration);
        PC.Damage /= multiplier;
        

        Destroy(gameObject);
    }
}
