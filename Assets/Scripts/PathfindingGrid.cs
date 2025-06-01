using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public GameObject nodePrefab;
    public int gridSizeX = 50;
    public int gridSizeZ = 50;
    public float nodeSpacing = 0.1f;
    public float centerX = 2f;
    public float centerZ = 0f;
    public float radius = 1.25f;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3 origin = new Vector3(centerX - (gridSizeX / 2f) * nodeSpacing, 1.011f, centerZ - (gridSizeZ / 2f) * nodeSpacing);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 pos = origin + new Vector3(x * nodeSpacing, 0f, z * nodeSpacing);
                if (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(centerX, centerZ)) <= radius)
                {
                    Instantiate(nodePrefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }
}
