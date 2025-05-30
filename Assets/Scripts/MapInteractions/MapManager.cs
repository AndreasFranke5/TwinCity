using UnityEngine;
using Fusion;

public class MapSyncController : NetworkBehaviour
{
    [SerializeField] private Transform rotatableMapBase;
    [SerializeField] private Transform waterPlane;
    [SerializeField] private Transform mapModel;

    [Networked] public float MapRotation { get; set; }
    [Networked] public float WaterLevel { get; set; }

    private Coroutine waterLevelCoroutine;
    private const float waterLerpDuration = 0.5f; // Duration in seconds

    public override void Spawned()
    {
        if (HasStateAuthority && rotatableMapBase != null && waterPlane != null)
        {
            WaterLevel = waterPlane.position.y - rotatableMapBase.position.y;
        }

        ApplyWaterLevel();
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && rotatableMapBase != null)
        {
            // Sync current rotation each tick
            MapRotation = rotatableMapBase.localEulerAngles.y;
        }
    }

    public override void Render()
    {
        if (mapModel != null)
        {
            var interpolator = new NetworkBehaviourBufferInterpolator(this);
            if (interpolator.Valid)
            {
                float interpolatedYRotation = interpolator.Float(nameof(MapRotation));
                mapModel.localRotation = Quaternion.Euler(0f, interpolatedYRotation, 0f);
            }
        }
    }

    private void ApplyWaterLevel()
    {
        if (rotatableMapBase != null && waterPlane != null)
        {
            Vector3 basePosition = rotatableMapBase.position;
            Vector3 targetPosition = new Vector3(basePosition.x, basePosition.y + WaterLevel, basePosition.z);

            if (waterLevelCoroutine != null)
                StopCoroutine(waterLevelCoroutine);

            waterLevelCoroutine = StartCoroutine(AnimateWaterLevel(waterPlane.position, targetPosition, waterLerpDuration));
        }
    }

    private System.Collections.IEnumerator AnimateWaterLevel(Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            waterPlane.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        waterPlane.position = endPos;
    }

    // Optional water level RPCs
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestWaterLevel(float level)
    {
        WaterLevel = level;
        ApplyWaterLevel();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_IncreaseWaterLevel(float amount)
    {
        WaterLevel += amount;
        ApplyWaterLevel();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DecreaseWaterLevel(float amount)
    {
        WaterLevel -= amount;
        ApplyWaterLevel();
    }
}
