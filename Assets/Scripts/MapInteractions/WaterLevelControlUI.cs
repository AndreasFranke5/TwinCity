using UnityEngine;

public class WaterLevelControlUI : MonoBehaviour
{
    [SerializeField] private MapSyncController mapSyncController;
    [SerializeField] private float changeAmount = 0.5f;

    // Called by UI button to increase water level
    public void IncreaseWaterLevel()
    {
        if (mapSyncController != null)
        {
            float newLevel = mapSyncController.WaterLevel + changeAmount;
            mapSyncController.RPC_RequestWaterLevel(newLevel);
        }
    }

    // Called by UI button to decrease water level
    public void DecreaseWaterLevel()
    {
        if (mapSyncController != null)
        {
            float newLevel = mapSyncController.WaterLevel - changeAmount;
            mapSyncController.RPC_RequestWaterLevel(newLevel);
        }
    }
}
