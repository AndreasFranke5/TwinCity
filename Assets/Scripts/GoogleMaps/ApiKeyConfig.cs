using UnityEngine;

[CreateAssetMenu(fileName = "ApiKeyConfig", menuName = "GoogleMaps/ApiKeyConfig")]
public class ApiKeyConfig : ScriptableObject
{
    [Header("🔑 API Key")]
    public string apiKey;

    [Header("🌍 URL Endpoints")]
    public string mapTilesBaseUrl = "https://tile.googleapis.com/v1/2dtiles";
    public string aerialViewBaseUrl = "https://aerialview.googleapis.com/v1";
}
