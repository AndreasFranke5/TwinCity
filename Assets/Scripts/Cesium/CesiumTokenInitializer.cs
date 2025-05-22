using UnityEngine;
using CesiumForUnity;

public class CesiumTokenInitializer : MonoBehaviour
{
    public CesiumTokenConfig tokenConfig;

    void Awake()
    {
        if (!string.IsNullOrEmpty(tokenConfig.cesiumIonToken))
            CesiumRuntimeSettings.defaultIonAccessToken = tokenConfig.cesiumIonToken;
    }
}
