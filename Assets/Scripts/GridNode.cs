using UnityEngine;

public class GridNode
{
    public bool isWalkable;
    public Vector3 WorldPosition { get; private set; }
    public int gridX;
    public int gridZ;

    public float gCost;
    public float hCost;
    public GridNode parent;

    public float fCost => gCost + hCost;

    public GridNode(bool isWalkable, Vector3 worldPos, int x, int z)
    {
        this.isWalkable = isWalkable;
        this.WorldPosition = worldPos;
        this.gridX = x;
        this.gridZ = z;
    }
}
