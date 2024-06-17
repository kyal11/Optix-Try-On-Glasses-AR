using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;

public class GlassesManager : MonoBehaviour
{
    public ARFaceManager arFaceManager;
    private string assetBundleUrl;
    private string prefabName;
    private string authToken;

    void Start()
    {
        GetIntentData();
        if (arFaceManager == null) Debug.LogError("ARFaceManager is not assigned!");
        StartCoroutine(DownloadAndLoadAssetBundle(assetBundleUrl, prefabName));
    }

    void GetIntentData()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent"))
                {
                    int productId = intent.Call<int>("getIntExtra", "productId", -1);
                    authToken = intent.Call<string>("getStringExtra", "authToken");
                    string apiUrl = intent.Call<string>("getStringExtra", "apiUrl");
                    assetBundleUrl = apiUrl + "/images_product/glassesbundle"; // Menggunakan apiUrl untuk assetBundleUrl
                    prefabName = productId.ToString(); // Menggunakan productId sebagai prefabName

                    Debug.Log("ProductId from Intent: " + productId);
                    Debug.Log("AuthToken from Intent: " + authToken);
                    Debug.Log("ApiUrl from Intent: " + apiUrl);
                }
            }
        }
    }

    IEnumerator DownloadAndLoadAssetBundle(string url, string assetName)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        www.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                yield break;
            }

            // Load the specified asset from the AssetBundle
            GameObject prefab = bundle.LoadAsset<GameObject>(assetName);
            if (prefab != null)
            {
                ChangeGlasses(prefab);
            }
            else
            {
                Debug.LogError($"Failed to load prefab '{assetName}' from AssetBundle!");
            }

            bundle.Unload(false);
        }
    }

    public void ChangeGlasses(GameObject newGlassesPrefab)
    {
        if (arFaceManager != null)
        {
            Debug.Log("Masuk ARFACEMANAGER");
            arFaceManager.facePrefab = newGlassesPrefab;
            ARFaceMeshVisualizer faceMeshVisualizer = newGlassesPrefab.GetComponent<ARFaceMeshVisualizer>();
            ARFace arFace = newGlassesPrefab.GetComponent<ARFace>();

            DestroyImmediate(faceMeshVisualizer, true);
            DestroyImmediate(arFace, true);
            newGlassesPrefab.AddComponent<ARFace>();
            newGlassesPrefab.AddComponent<ARFaceMeshVisualizer>();
        }
    }
}
