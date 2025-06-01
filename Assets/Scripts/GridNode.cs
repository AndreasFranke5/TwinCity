using UnityEngine;

public class GridNode : MonoBehaviour
{
    public int gridX;
    public int gridZ;
    public bool isWalkable = true;
    public float cost = 1f;

    public GridNode parent;
    public float gCost;
    public float hCost;

    public float FCost => gCost + hCost;
}
