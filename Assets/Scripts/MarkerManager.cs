using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MarkerManager : MonoBehaviour
{
    public Pathfinder pathfinder;

    private List<GameObject> markers = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(RefreshMarkers), 1f, 0.5f);
    }

    void RefreshMarkers()
    {
        GameObject[] found = GameObject.FindGameObjectsWithTag("Marker");
        if (found.Length != markers.Count)
        {
            markers = found.OrderBy(m => m.transform.GetSiblingIndex()).ToList();

            if (markers.Count >= 2)
            {
                GameObject markerA = markers[markers.Count - 2];
                GameObject markerB = markers[markers.Count - 1];
                pathfinder.DrawPath(markerA.transform.position, markerB.transform.position);
            }
            else
            {
                pathfinder.ClearCurrentPath();
            }
        }
    }

    public void RemoveAllMarkers()
    {
        foreach (GameObject marker in markers)
            Destroy(marker);

        markers.Clear();
        pathfinder.ClearCurrentPath();
    }
}
