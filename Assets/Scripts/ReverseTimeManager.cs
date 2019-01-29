using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

    public GameObject player;

    public ArrayList playerPositions;
    public ArrayList playerRotations;

    public bool isReversing = false;

	// Use this for initialization
	void Start () {

        playerPositions = new ArrayList();
        playerRotations = new ArrayList();

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isReversing = true;
        }
        else
        {
            isReversing = false;
        }
    }

    void FixedUpdate()
    {
        if (!isReversing)
        {
            playerPositions.Add(player.transform.position);
            playerRotations.Add(player.transform.localEulerAngles);
        }
        else
        {
            player.transform.position = (Vector3)playerPositions[playerPositions.Count - 1];
            playerPositions.RemoveAt(playerPositions.Count - 1);

            player.transform.localEulerAngles = (Vector3)playerRotations[playerRotations.Count - 1];
            playerRotations.RemoveAt(playerRotations.Count - 1);
        }
    }


}
