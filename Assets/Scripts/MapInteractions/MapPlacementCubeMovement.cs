using UnityEngine;
using Oculus.Interaction;

public class MapPlacementCubeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public InteractableUnityEventWrapper interactable;

    private float fixedY;
    private float fixedX;
    private float fixedZ;
    private bool isGrabbed = false;

    void Start()
    {
        fixedY = transform.position.y;
        fixedX = transform.position.x;
        fixedZ = transform.position.z;

        if (interactable != null)
        {
            interactable.WhenSelect.AddListener(OnGrab);
            interactable.WhenUnselect.AddListener(OnRelease);
        }
        else
        {
            Debug.LogWarning("InteractableUnityEventWrapper not assigned on " + gameObject.name);
        }
    }

    void Update()
    {
        if (!isGrabbed)
        {
            transform.position = new Vector3(fixedX, fixedY, fixedZ);
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(h, 0, v).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, fixedY, pos.z);

            fixedX = pos.x;
            fixedZ = pos.z;
        }

        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnGrab()
    {
        isGrabbed = true;
    }

    private void OnRelease()
    {
        Vector3 pos = transform.position;
        fixedX = pos.x;
        fixedZ = pos.z;
        isGrabbed = false;
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.WhenSelect.RemoveListener(OnGrab);
            interactable.WhenUnselect.RemoveListener(OnRelease);
        }
    }
}
