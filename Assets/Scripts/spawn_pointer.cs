using UnityEngine;
using Quaternion = UnityEngine.Quaternion;


public class spawn_pointer : MonoBehaviour
{
    public GameObject pointer;

    public Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPointer()
    {
        Instantiate(pointer, spawnPoint.position, Quaternion.identity);
    }
}
