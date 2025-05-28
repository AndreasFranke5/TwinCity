using UnityEngine;

public class spawn_pointer : MonoBehaviour
{
    public GameObject pointerPrefab;
    public GameObject yellowPrefeb;

    public Transform spawnPoint;

    void Start()
    {
        // Optional: Validate the spawn point
        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point not assigned!");
        }
    }

    void Update()
    {
        // Optional: Trigger spawn for testing
        // if (Input.GetKeyDown(KeyCode.Space)) SpawnPointer();
    }

    public void SpawnPointer()
    {
        if (pointerPrefab != null && spawnPoint != null)
        {
            Instantiate(pointerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Pointer prefab or spawn point not assigned!");
        }
    }

    public void SpawnPointer1()
    {
        if (yellowPrefeb != null && spawnPoint != null)
        {
            Instantiate(yellowPrefeb, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Yellow prefab or spawn point not assigned!");
        }
    }
}
