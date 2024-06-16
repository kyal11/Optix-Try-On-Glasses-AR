using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PrefabLoader : MonoBehaviour
{
    private string apiUrl = "http://100.97.75.94:8080/products/352";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ5b2RoYW5hYmloYTJAZ21haWwuY29tIiwidXNlcm5hbWUiOiJ5b2RoYW5hYmloYTJAZ21haWwuY29tIn0.1GYZYS3i7fEyDbd1xcaM1LV7f5UDNYk6rjNzP_Y0ipo";

    void Start()
    {
        StartCoroutine(GetPrefabFromServer());
    }

    IEnumerator GetPrefabFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        www.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to get prefab URL: " + www.error);
        }
        else
        {
            string prefabUrl = www.downloadHandler.text;
            Debug.Log("Received prefab URL from API: " + prefabUrl);
            StartCoroutine(DownloadAndInstantiatePrefab(prefabUrl));
        }
    }

    IEnumerator DownloadAndInstantiatePrefab(string prefabUrl)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(prefabUrl);
        www.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to download asset bundle: " + www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            Debug.Log("Asset bundle downloaded successfully: " + bundle);
            //GameObject prefab = bundle.LoadAsset<GameObject>("PrefabName");
            //Instantiate(prefab);
        }
    }
}
