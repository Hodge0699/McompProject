using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script needs to be connect to an empty object
//Make sure gizmos are visible!

namespace Enemy
{
	public class EnemiesSpawn : MonoBehaviour
	{
	    public Vector3 center;
	    public Vector3 size;

	    public GameObject Enemyprefab;
	    public GameObject floor;

	    // Use this for initialization
	    void Start()
	    {
	        //Spawn();
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        //if (Input.GetKey(KeyCode.O))
	        //{
	        //    SpawnBounds();
	        //}
	    }

	    public void SpawnBounds()
	    {
	    	floor = GameObject.Find("Floor");

	    	center = floor.transform.position;
	    	center.y = center.y + 1;

	        size = floor.GetComponent<Renderer>().bounds.size;
	        size.x = size.x - 5;
	        size.z = size.z - 5;

	        Spawn();
	    }

	    public GameObject Spawn()
	    {
	        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0.0f, Random.Range(-size.z / 2, size.z / 2));

	        // Vector2 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.z / 2, size.z / 2));
	        
	        return Instantiate(Enemyprefab, pos, Quaternion.identity);
	    }


	    void OnDrawGizmosSelected()
	    {
	        Gizmos.color = new Color(1, 0, 0, 0.5f);
	        Gizmos.DrawCube(center, size);
	        Gizmos.DrawCube(transform.localPosition, size);
	    }
	}
}
