using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public int gridSizeX = 30;
    public int gridSizeZ = 30;
    public float nodeSpacing = 0.1f;
    public Vector3 origin = new Vector3(0.75f, 1.011f, -1.25f);
    public float radius = 1.25f;
    public GameObject nodePrefab;

    private GridNode[,] grid;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GridNode[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = origin + new Vector3(x * nodeSpacing, 0f, z * nodeSpacing);
                bool walkable = IsInsideCircle(worldPoint);
                grid[x, z] = new GridNode(walkable, worldPoint, x, z);

                if (nodePrefab != null && walkable)
                {
                    Instantiate(nodePrefab, worldPoint, Quaternion.identity);
                }
            }
        }
    }

    bool IsInsideCircle(Vector3 point)
    {
        Vector3 center = origin + new Vector3(gridSizeX * nodeSpacing / 2f, 0, gridSizeZ * nodeSpacing / 2f);
        float dist = Vector3.Distance(new Vector3(point.x, 0, point.z), new Vector3(center.x, 0, center.z));
        return dist <= radius;
    }

    public GridNode GetNearestNode(Vector3 worldPos)
    {
        float percentX = Mathf.Clamp01((worldPos.x - origin.x) / (gridSizeX * nodeSpacing));
        float percentZ = Mathf.Clamp01((worldPos.z - origin.z) / (gridSizeZ * nodeSpacing));
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, z];
    }

    public List<GridNode> GetAllNodes()
    {
        List<GridNode> allNodes = new List<GridNode>();
        foreach (var node in grid)
            allNodes.Add(node);
        return allNodes;
    }

    public List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;
                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                    neighbors.Add(grid[checkX, checkZ]);
            }
        }
        return neighbors;
    }
}
