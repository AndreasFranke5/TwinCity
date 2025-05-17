using UnityEngine;
using Fusion;

public class MapSyncController : NetworkBehaviour
{
    [SerializeField] private Transform rotatableMapBase;
    [SerializeField] private Transform waterPlane;

    [Networked] public float MapRotation { get; set; }
    [Networked] public float WaterLevel { get; set; }

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && rotatableMapBase != null)
        {
            // Read the Y-axis rotation of the base and write it to the networked variable
            float currentYRotation = rotatableMapBase.localEulerAngles.y;
            MapRotation = currentYRotation;
        }
    }

    public override void Render()
    {
        foreach (var property in _changeDetector.DetectChanges(this))
        {
            switch (property)
            {
                case nameof(MapRotation):
                    ApplyRotation();
                    break;
                case nameof(WaterLevel):
                    ApplyWaterLevel();
                    break;
            }
        }
    }

    private void ApplyRotation()
    {
        if (waterPlane != null)
        {
            waterPlane.localRotation = Quaternion.Euler(0f, MapRotation, 0f);
        }
    }

    private void ApplyWaterLevel()
    {
        if (rotatableMapBase != null && waterPlane != null)
        {
            Vector3 basePosition = rotatableMapBase.position;
            waterPlane.position = new Vector3(basePosition.x, basePosition.y + WaterLevel, basePosition.z);
        }
    }

    // Clients can still request water level change if needed
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestWaterLevel(float level)
    {
        WaterLevel = level;
    }
}
