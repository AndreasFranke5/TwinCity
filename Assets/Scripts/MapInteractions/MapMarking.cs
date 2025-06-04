using System.Collections.Generic;
using UnityEngine;

public class MarkerRaycastToMap : MonoBehaviour
{
    [Header("Marker Settings")]
    public string markerTag = "Marker";
    public float rayLength = 2000f;
    public LayerMask mapLayer;

    [Header("Ray Appearance")]
    public Material rayMaterial;
    public float rayWidth = 0.02f;

    private readonly List<LineRenderer> activeRays = new List<LineRenderer>();

    void Update()
    {
        ClearOldRays();

        GameObject[] markers = GameObject.FindGameObjectsWithTag(markerTag);

        foreach (GameObject marker in markers)
        {
            if (marker == null) continue;

            Renderer rend = marker.GetComponent<Renderer>();
            if (rend == null) continue;

            Bounds bounds = rend.bounds;
            Vector3 origin = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            Vector3 direction = Vector3.down;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength, mapLayer))
            {
                DrawRay(origin, hit.point);
            }
        }
    }

    void DrawRay(Vector3 start, Vector3 end)
    {
        GameObject rayObj = new GameObject("MarkerToMapRay");
        rayObj.transform.parent = this.transform;

        LineRenderer lr = rayObj.AddComponent<LineRenderer>();
        lr.material = rayMaterial ?? new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.startWidth = rayWidth;
        lr.endWidth = rayWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        activeRays.Add(lr);
    }

    void ClearOldRays()
    {
        foreach (var lr in activeRays)
        {
            if (lr != null)
                Destroy(lr.gameObject);
        }
        activeRays.Clear();
    }
}
