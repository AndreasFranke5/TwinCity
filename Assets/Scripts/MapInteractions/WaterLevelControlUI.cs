using UnityEngine;

public class WaterLevelControlUI : MonoBehaviour
{
    [SerializeField] private MapSyncController mapSyncController;
    [SerializeField] public float changeAmount = 0.01f;

    public void IncreaseWaterLevel()
    {
        if (mapSyncController != null)
        {
            mapSyncController.RPC_IncreaseWaterLevel(changeAmount);
        }
    }

    public void DecreaseWaterLevel()
    {
        if (mapSyncController != null)
        {
            mapSyncController.RPC_DecreaseWaterLevel(changeAmount);
        }
    }
}
