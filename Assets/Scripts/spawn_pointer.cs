using UnityEngine;
using Fusion;
using System.Collections.Generic;

public enum MarkerType : byte
{
    Red,
    Yellow
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
    public Transform spawnPoint;

    private Dictionary<int, GameObject> spawnedMarkers = new(); // Tracks instances by ID
    private Dictionary<int, MarkerData> markerDataStore = new(); // Only on host
    private int markerIdCounter = 0; // Only on host

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
        if (prefab == null) return;

        GameObject instance = Instantiate(prefab, position, Quaternion.identity, spawnPoint?.parent);
        spawnedMarkers[id] = instance;
         
        var interaction = instance.GetComponent<MarkerInteraction>();
        if (interaction != null)
        {
            interaction.markerId = id;
            interaction.spawnManager = this;
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

}
