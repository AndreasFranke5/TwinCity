using UnityEngine;
using Fusion;

public class MapPlacement : NetworkBehaviour
{
    [Header("Reference to the networked cube")]
    public NetworkObject targetCube;

    [Header("Offset to align map relative to cube")]
    public float xOffset = 0f;
    public float zOffset = 0f;

    void Update()
    {
        if (targetCube != null)
        {
            Vector3 targetPosition = targetCube.transform.position;
            Vector3 currentPosition = transform.position;

            // Apply X and Z offsets while keeping current Y
            transform.position = new Vector3(
                targetPosition.x + xOffset,
                currentPosition.y,
                targetPosition.z + zOffset
            );
        }
    }
}
