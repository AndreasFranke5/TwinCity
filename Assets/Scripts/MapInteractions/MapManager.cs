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
    private float lastSentRotation = -1f;
    private Quaternion targetRotation;

    public override void Spawned()
    {
        Debug.LogWarning("SPAWNEED");
        if (HasStateAuthority && rotatableMapBase != null && waterPlane != null)
        {
            WaterLevel = waterPlane.position.y - rotatableMapBase.position.y;
        }

        ApplyWaterLevel();
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || rotatableMapBase == null) return;

        float currentY = rotatableMapBase.localEulerAngles.y;
        if (Mathf.Abs(currentY - lastSentRotation) > 0.1f)
        {
            MapRotation = currentY;
            lastSentRotation = currentY;
        }
    }

    public override void Render()
    {
        if (mapModel != null)
        {
            mapModel.localRotation = Quaternion.Euler(0f, MapRotation, 0f);

            if (!HasStateAuthority)
            {
                //Debug.LogWarning("NO STATE AUTH; WERE MOVING BASE");
                rotatableMapBase.localRotation = Quaternion.Euler(0f, MapRotation, 0f);
            }
        }
    }

    public void RequestAuthority()
    {
        Object.RequestStateAuthority();
    }

    public void ReleaseAuthority()
    {
        Object.ReleaseStateAuthority();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, InvokeLocal = false)]
    private void RPC_StreamRotation(float newRotationY)
    {
        MapRotation = newRotationY;
    }

    public void SendLiveRotation(float newYRotation)
    {
        RPC_StreamRotation(newYRotation);
        if (Object.HasInputAuthority)
        {
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
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RequestWaterLevel(float level)
    {
        WaterLevel = level;
        ApplyWaterLevel();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_IncreaseWaterLevel(float amount)
    {
        WaterLevel += amount;
        ApplyWaterLevel();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_DecreaseWaterLevel(float amount)
    {
        WaterLevel -= amount;
        ApplyWaterLevel();
    }
}
