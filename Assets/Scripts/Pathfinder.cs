using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public PathfindingGrid grid;
    public string redMarkerTag = "RedMarker";
    public string yellowMarkerTag = "YellowMarker";
    public Material lineMaterial;

    private LineRenderer lineRenderer;
    private Transform redMarker;
    private Transform yellowMarker;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 0;

        InvokeRepeating(nameof(CheckForMarkers), 1f, 0.5f); // Check every 0.5 seconds
    }

    void CheckForMarkers()
    {
        redMarker = GameObject.FindWithTag(redMarkerTag)?.transform;
        yellowMarker = GameObject.FindWithTag(yellowMarkerTag)?.transform;

        if (redMarker != null && yellowMarker != null)
        {
            ComputePath(redMarker, yellowMarker);
        }
        else
        {
            ClearPath();
        }
    }

    void ComputePath(Transform startMarker, Transform endMarker)
    {
        if (grid == null)
        {
            Debug.LogError("Pathfinder: Grid reference missing.");
            return;
        }

        GridNode startNode = grid.GetNearestNode(startMarker.position);
        GridNode endNode = grid.GetNearestNode(endMarker.position);

        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (GridNode neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    continue;

                float newCostToNeighbor = currentNode.gCost + Vector3.Distance(currentNode.WorldPosition, neighbor.WorldPosition);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = Vector3.Distance(neighbor.WorldPosition, endNode.WorldPosition);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        Debug.LogWarning("Pathfinder: No path found.");
        ClearPath();
    }

    void RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();

        Vector3[] points = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            points[i] = path[i].WorldPosition + Vector3.up * 0.05f;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    void ClearPath()
    {
        lineRenderer.positionCount = 0;
    }
}
