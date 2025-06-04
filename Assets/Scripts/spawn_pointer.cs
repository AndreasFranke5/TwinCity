using UnityEngine;
using Fusion;
using System.Collections.Generic;

public enum MarkerType : byte
{
    Red,
    Yellow,
    GreenLine
}

[System.Serializable]
public struct MarkerData
{
    public int ID;
    public MarkerType Type;
    public Vector3 Position;
}

public class spawn_pointer : NetworkBehaviour
{
    public GameObject pointerPrefab;
    public GameObject yellowPrefab;
    public GameObject greenPrefab;
    public GameObject linePrefab;
    public Transform spawnPoint;

    public Material lineMaterial;
    public float lineWidth = 0.02f;

    private Dictionary<int, GameObject> spawnedMarkers = new(); // Tracks instances by ID
    private Dictionary<int, MarkerData> markerDataStore = new(); // Only on host
    private int markerIdCounter = 0; // Only on host

    private GameObject pendingLineMarker = null;
    private readonly List<(GameObject A, GameObject B, LineRenderer LR)> linePairs = new();

    public override void Spawned()
    {
        if (!HasStateAuthority) return;

        // If desired, preload existing markers here
    }

    public void SpawnPointer()
    {
        if (spawnPoint != null)
            RPC_RequestSpawnMarker(MarkerType.Red, spawnPoint.position);
    }

    public void SpawnPointer1()
    {
        if (spawnPoint != null)
            RPC_RequestSpawnMarker(MarkerType.Yellow, spawnPoint.position);
    }

    public void SpawnLineMarkerGreen()
    {
        if (spawnPoint != null)
            RPC_RequestSpawnMarker(MarkerType.GreenLine, spawnPoint.position);
    }

    private static Vector3 AnchorPoint(GameObject go)
    {
        Renderer rend = go.GetComponent<Renderer>();

        Bounds bounds = rend.bounds;
        return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestSpawnMarker(MarkerType type, Vector3 position)
    {
        int id = markerIdCounter++;
        markerDataStore[id] = new MarkerData { ID = id, Type = type, Position = position };

        RPC_SpawnMarker(id, type, position);
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SpawnMarker(int id, MarkerType type, Vector3 position)
    {
        GameObject prefab = type == MarkerType.Red ? pointerPrefab : yellowPrefab;
        if (type == MarkerType.GreenLine)
            prefab = greenPrefab;
        if (prefab == null) return;

        GameObject instance = Instantiate(prefab, position, Quaternion.identity, spawnPoint?.parent);
        spawnedMarkers[id] = instance;
         
        var interaction = instance.GetComponent<MarkerInteraction>();
        if (interaction != null)
        {
            interaction.markerId = id;
            interaction.spawnManager = this;
        }

        if (type == MarkerType.GreenLine)
        {
            if (pendingLineMarker == null)
            {
                pendingLineMarker = instance;
            }
            else
            {
                CreateLineBetween(pendingLineMarker, instance);
                pendingLineMarker = null;
            }
        }
    }


    public void MoveMarker(int id, Vector3 newPosition)
    {
        if (HasStateAuthority && markerDataStore.ContainsKey(id))
        {
            MarkerData updated = markerDataStore[id];
            updated.Position = newPosition;
            markerDataStore[id] = updated;

            RPC_UpdateMarkerPosition(id, newPosition);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdateMarkerPosition(int id, Vector3 newPosition)
    {
        if (spawnedMarkers.TryGetValue(id, out GameObject marker))
        {
            marker.transform.position = newPosition;
        }
    }

    public void SendMarkerUpdateToHost(int id, Vector3 newPosition)
    {
        RPC_ClientRequestsMove(id, newPosition);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_ClientRequestsMove(int id, Vector3 newPosition)
    {
        MoveMarker(id, newPosition);
    }

    private void CreateLineBetween(GameObject a, GameObject b)
    {
        var line = Instantiate(linePrefab);
        var lineLR = line.gameObject.GetComponent<LineRenderer>();
        lineLR.SetPosition(0, AnchorPoint(a));
        lineLR.SetPosition(1, AnchorPoint(b));
        linePairs.Add((a, b, lineLR));
        /*
        GameObject lineObj = new GameObject($"Line_{a.GetInstanceID()}_{b.GetInstanceID()}");
        lineObj.transform.parent = transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial ?? new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = Color.green;
        lr.startWidth = lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.alignment = LineAlignment.TransformZ;
        lr.SetPosition(0, AnchorPoint(a));
        lr.SetPosition(1, AnchorPoint(b));
        Vector3 dir = AnchorPoint(b) - AnchorPoint(a);
        lineObj.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        linePairs.Add((a, b, lr));
        */
    }

    private void Update()
    {
        foreach (var (A, B, LR) in linePairs)
        {
            if (A && B && LR)
            {
                LR.SetPosition(0, AnchorPoint(A));
                LR.SetPosition(1, AnchorPoint(B));
                //Vector3 dir = AnchorPoint(B) - AnchorPoint(A);
                //LR.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
        }
    }
}
