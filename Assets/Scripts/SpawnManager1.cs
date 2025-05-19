using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SpawnManager1 : MonoBehaviour
{

public static SpawnManager1 Instance;
public GameObject pointerPrefab;
private static readonly Vector3 SpawnPosition = new Vector3(0,0,0);
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
        Instantiate(pointerPrefab,SpawnPosition,Quaternion.identity);
    }
}
