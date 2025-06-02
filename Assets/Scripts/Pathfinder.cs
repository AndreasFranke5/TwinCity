using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public PathfindingGrid grid;
    public Material lineMaterial;

    private LineRenderer currentLine;

    public void DrawPath(Vector3 startWorldPos, Vector3 endWorldPos)
    {
        GridNode startNode = grid.GetNearestNode(startWorldPos);
        GridNode endNode = grid.GetNearestNode(endWorldPos);
        if (startNode == null || endNode == null) return;

        List<GridNode> path = AStar(startNode, endNode);
        if (path == null || path.Count == 0) return;

        if (currentLine == null)
        {
            GameObject lineObj = new GameObject("PathLine");
            currentLine = lineObj.AddComponent<LineRenderer>();
            currentLine.material = lineMaterial;
            currentLine.startWidth = 0.05f;
            currentLine.endWidth = 0.05f;
            currentLine.useWorldSpace = true;
            currentLine.positionCount = 0;
        }

        currentLine.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
            currentLine.SetPosition(i, path[i].WorldPosition);
    }

    public void ClearCurrentPath()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
    }

    private List<GridNode> AStar(GridNode start, GridNode end)
    {
        var openSet = new List<GridNode> { start };
        var cameFrom = new Dictionary<GridNode, GridNode>();
        var gScore = new Dictionary<GridNode, float>();
        var fScore = new Dictionary<GridNode, float>();

        foreach (var node in grid.GetAllNodes())
        {
            gScore[node] = float.MaxValue;
            fScore[node] = float.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);

        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            GridNode current = openSet[0];
            if (current == end)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            foreach (var neighbor in grid.GetNeighbors(current))
            {
                if (!neighbor.isWalkable) continue;

                float tentativeG = gScore[current] + Vector3.Distance(current.WorldPosition, neighbor.WorldPosition);
                if (tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null;
    }

    private float Heuristic(GridNode a, GridNode b)
    {
        return Vector3.Distance(a.WorldPosition, b.WorldPosition);
    }

    private List<GridNode> ReconstructPath(Dictionary<GridNode, GridNode> cameFrom, GridNode current)
    {
        List<GridNode> path = new List<GridNode> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}
