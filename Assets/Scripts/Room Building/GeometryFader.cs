using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryFader : MonoBehaviour {

    private List<GameObject> obstructedObjects = new List<GameObject>();

    private float maxDist;

    private MeshRenderer mesh;

	// Use this for initialization
	void Start () {
        maxDist = GetComponent<BoxCollider>().size.z * 1.5f;
        mesh = transform.parent.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        // Make sure all objects in list are valid
        validateObjects();

        float alpha;

        if (obstructedObjects.Count > 0)
        {
            alpha = getClosestObjectDist() / maxDist;

            if (alpha < 0.2f)
                alpha = 0.2f;
            else if (alpha > 1.0f)
                alpha = 1.0f;
        }
        else
            alpha = 1.0f;

        Color meshCol = mesh.material.color;
        mesh.material.color = new Color(meshCol.r, meshCol.g, meshCol.b, alpha);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null || other.GetComponent<EnemyType.AbstractEnemy>() != null || other.GetComponent<Powerups.AbstractPowerup>() != null)
        {
            if (!obstructedObjects.Contains(other.gameObject))
                obstructedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (obstructedObjects.Contains(other.gameObject))
            obstructedObjects.Remove(other.gameObject);
    }

    /// <summary>
    /// Removes destroyed objects from obstructedObjects
    /// </summary>
    private void validateObjects()
    {
        List<int> nullIndices = new List<int>();

        // Find destroyed
        for (int i = 0; i < obstructedObjects.Count; i++)
        {
            if (obstructedObjects[i] == null)
                nullIndices.Add(i);
        }

        // Remove destroyed
        for (int i = nullIndices.Count - 1; i >= 0; i--)
            obstructedObjects.RemoveAt(i);
    }

    /// <summary>
    /// Gets the closest obscured object to the parent and returns its distance
    /// </summary>
    private float getClosestObjectDist()
    {
        float parentZ = transform.parent.transform.position.z;

        float minZ = Mathf.Abs(obstructedObjects[0].transform.position.z - parentZ);

        for (int i = 1; i < obstructedObjects.Count; i++)
        {
            float dist = Mathf.Abs(obstructedObjects[i].transform.position.z - parentZ);

            if (dist < minZ)
                minZ = dist;
        }

        return minZ;
    }

    private void OnDestroy()
    {
        Color meshCol = mesh.material.color;
        mesh.material.color = new Color(meshCol.r, meshCol.g, meshCol.b, 1.0f);
    }
}
