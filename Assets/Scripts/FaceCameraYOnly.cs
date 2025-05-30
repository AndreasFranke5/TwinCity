using UnityEngine;

public class FaceCameraYOnly : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogWarning("Main Camera not found. Make sure your HMD camera is tagged as 'MainCamera'.");
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Calculate direction to camera, but only in horizontal plane
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        // Look in horizontal direction, keep object upright
        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
    }
}
