using UnityEngine;

public class PrintCirclePoints : MonoBehaviour
{
    public int points = 20;
    public float radius = 1.25f; // Half of scale.x (if scale is uniform)
    public float y = 1.011f; // Height at the top of the cylinder
    public Vector3 center = new Vector3(2, 1.011f, 0); // Cylinder center in world space

    void Start()
    {
        for (int i = 0; i < points; i++)
        {
            float angle = 2 * Mathf.PI * i / points;
            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);
            Debug.Log($"Knot {i}: X={x}, Y={y}, Z={z}");
        }
    }
}
