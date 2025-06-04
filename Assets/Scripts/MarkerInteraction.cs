using UnityEngine;
using Fusion;
using Oculus.Interaction;

public class MarkerInteraction : NetworkBehaviour
{
    [Tooltip("Reference to the spawn_pointer script handling networking.")]
    public spawn_pointer spawnManager;

    [Tooltip("Unique ID of this marker, assigned at spawn.")]
    public int markerId;

    private void Awake()
    {
        var interactableWrapper = GetComponent<InteractableUnityEventWrapper>();

        if (interactableWrapper != null)
        {
            interactableWrapper.WhenUnselect.AddListener(OnGrabReleased);
        }
        else
        {
            Debug.LogWarning("InteractableUnityEventWrapper not found on marker object.");
        }
    }

    private void OnDestroy()
    {
        var interactableWrapper = GetComponent<InteractableUnityEventWrapper>();

        if (interactableWrapper != null)
        {
            interactableWrapper.WhenUnselect.RemoveListener(OnGrabReleased);
        }
    }

    private void OnGrabReleased()
    {
        // Anyone can send an update, even if they're not the host
        if (spawnManager != null)
        {
            spawnManager.SendMarkerUpdateToHost(markerId, transform.position);
        }
    }
}
