using UnityEngine;
using Oculus.Interaction;

public class MapRotationBroadcaster : MonoBehaviour
{
    [SerializeField] private MapSyncController mapSync;
    [SerializeField] private Transform mapBase; // The base being rotated

    private InteractableUnityEventWrapper interactable;
    private bool isBeingGrabbed = false;
    private float sendInterval = 0.1f;
    private float lastSentTime = 0f;

    private void Awake()
    {
        interactable = GetComponentInChildren<InteractableUnityEventWrapper>();
        if (interactable != null)
        {
            interactable.WhenSelect.AddListener(() => OnGrab());
            interactable.WhenUnselect.AddListener(() => OnRelease());
        }
    }

    private void Update()
    {
        if (isBeingGrabbed && mapSync != null) //&& mapSync.HasInputAuthority
        {
            if (Time.time - lastSentTime > sendInterval)
            {
                float yRot = mapBase.localEulerAngles.y;
                mapSync.SendLiveRotation(yRot);
                lastSentTime = Time.time;
            }
        }
    }

    private void OnGrab()
    {
        isBeingGrabbed = true;

        if (mapSync != null)
        {
            Debug.Log("Request StateAuthority");
            mapSync.RequestAuthority();
        }
    }

    private void OnRelease()
    {
        isBeingGrabbed = false;

        if (mapSync != null)
        {
            Debug.Log("Release StateAuthority");
            mapSync.ReleaseAuthority();
        }
    }
}
