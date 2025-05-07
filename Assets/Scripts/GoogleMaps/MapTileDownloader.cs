using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MapTileDownloader : MonoBehaviour
{
    public ApiKeyConfig config;
    void Awake()
    {
        if (config == null)
            Debug.LogError("❌ ApiKeyConfig is not assigned in MapTileDownloader!");
    }

    public void DownloadTile(string mapId, int x, int y, int zoom, System.Action<Texture2D> onTileReady)
    {
        string url = $"{config.mapTilesBaseUrl}/{zoom}/{x}/{y}?key={config.apiKey}&mapId={mapId}";

        StartCoroutine(DownloadCoroutine(url, onTileReady));
    }

    private IEnumerator DownloadCoroutine(string url, System.Action<Texture2D> callback)
    {
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Tile download failed: " + www.error);
        }
        else
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            callback?.Invoke(tex);
        }
    }
}
