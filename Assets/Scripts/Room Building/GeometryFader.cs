using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomBuilding
{
    public class GeometryFader : MonoBehaviour
    {
        public float maxAlpha = 1.0f;
        public float minAlpha = 0.2f;

        private List<GameObject> obstructedObjects = new List<GameObject>();

        private float maxDist;

        private MeshRenderer mesh;

        private enum RenderMode { OPAQUE, FADE };

        // Use this for initialization
        void Start()
        {
            maxDist = transform.parent.lossyScale.y * 2;

            BoxCollider trigger = GetComponent<BoxCollider>();
            trigger.size = new Vector3(trigger.size.x, trigger.size.y, maxDist);

            mesh = transform.parent.GetComponent<MeshRenderer>();

            setRenderMode(RenderMode.FADE);
        }

        // Update is called once per frame
        void Update()
        {

            // Make sure all objects in list are valid
            validateObjects();

            float alpha;

            if (obstructedObjects.Count > 0)
            {
                alpha = getClosestObjectDist() / maxDist;

                alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
            }
            else
                alpha = 1.0f;

            Color meshCol = mesh.material.color;
            mesh.material.color = new Color(meshCol.r, meshCol.g, meshCol.b, alpha);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player.PlayerController>() != null || other.GetComponent<EnemyType.AbstractEnemy>() != null || other.GetComponent<Powerups.AbstractPowerup>() != null)
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
        /// Sets the render mode to Fade
        /// </summary>
        /// <param name="renderMode">The Rendering mode to use.</param>
        private void setRenderMode(RenderMode renderMode)
        {
            switch (renderMode)
            {
                case (RenderMode.OPAQUE):
                    Color meshCol = mesh.material.color;
                    mesh.material.color = new Color(meshCol.r, meshCol.g, meshCol.b, 1.0f);
                    mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    mesh.material.SetInt("_ZWrite", 1);
                    mesh.material.DisableKeyword("_ALPHATEST_ON");
                    mesh.material.DisableKeyword("_ALPHABLEND_ON");
                    mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mesh.material.renderQueue = -1;
                    break;
                case (RenderMode.FADE):
                    mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mesh.material.SetInt("_ZWrite", 0);
                    mesh.material.DisableKeyword("_ALPHATEST_ON");
                    mesh.material.EnableKeyword("_ALPHABLEND_ON");
                    mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mesh.material.renderQueue = 3000;
                    break;
            }

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
                obstructedObjects.RemoveAt(nullIndices[i]);
        }

        /// <summary>
        /// Gets the closest obscured object to the parent and returns its distance
        /// </summary>
        private float getClosestObjectDist()
        {
            float parentZ = transform.parent.transform.position.z;

            float minZ = Mathf.Infinity;

            for (int i = 0; i < obstructedObjects.Count; i++)
            {
                if (obstructedObjects[i] != null)
                {
                    float dist = getDistance(obstructedObjects[i].transform);

                    if (dist < minZ)
                        minZ = dist;
                }
            }

            return minZ;
        }

        /// <summary>
        /// Calculates the distance from a point to this wall
        /// </summary>
        /// <param name="other">Other transform to measure to</param>
        /// <returns>The closest distance from this wall to other</returns>
        private float getDistance(Transform other)
        {
            Vector2 thisPos = new Vector2(transform.parent.position.x, transform.parent.position.z);
            float wallLength = transform.parent.lossyScale.x;

            Vector2 otherPos = new Vector2(other.position.x, other.position.z);

            Vector2 wallRight = new Vector2(transform.right.x, transform.right.y);

            Vector2 wallStart = thisPos + (wallRight * (wallLength / 2));
            Vector2 wallEnd = thisPos + (-wallRight * (wallLength / 2));

            float dx = wallEnd.x - wallStart.x;
            float dy = wallEnd.y - wallStart.y;

            return Mathf.Abs(dy * otherPos.x - dx * otherPos.y - wallStart.x * wallEnd.y + wallEnd.x * wallStart.y) / Mathf.Sqrt((dx * dx) + (dy * dy));
        }

        private void OnDestroy()
        {
            setRenderMode(RenderMode.OPAQUE);
        }
    }
}