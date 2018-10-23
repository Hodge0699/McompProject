using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseTimeScene : MonoBehaviour {

    [SerializeField]
    private GameObject Player = null;


    private void Awake()
    {
        Instantiate(Player, transform.position, Quaternion.identity);
    }
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
