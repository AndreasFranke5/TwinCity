using UnityEngine;

public class MapTileManager : MonoBehaviour
{
    public MapTileDownloader tileDownloader;
    public string mapId = "your-map-id";  // Create in Google Cloud if needed
    public int zoom = 16;
    public int centerX = 34567;
    public int centerY = 22123;
    public int tileCount = 3;
    public float tileSize = 10f;

    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        for (int x = -tileCount / 2; x <= tileCount / 2; x++)
        {
            for (int y = -tileCount / 2; y <= tileCount / 2; y++)
            {
                int tileX = centerX + x;
                int tileY = centerY + y;

                Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);

                tileDownloader.DownloadTile(mapId, tileX, tileY, zoom, texture =>
                {
                    GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    tile.transform.localScale = Vector3.one * tileSize * 0.1f; // Plane is 10x10 by default
                    tile.transform.position = position;

                    Material mat = new Material(Shader.Find("Standard"));
                    mat.mainTexture = texture;
                    tile.GetComponent<Renderer>().material = mat;
                });
            }
        }
    }
}
