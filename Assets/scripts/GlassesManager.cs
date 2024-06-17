using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;

public class GlassesManager : MonoBehaviour
{
    public ARFaceManager arFaceManager;
    public string assetBundleUrl = "http://100.97.75.94:8080/images_product/glassesbundle";
    public string prefabName = "Tracking Glasses 3"; // Nama prefab yang ingin dimuat

    void Start()
    {
        if (arFaceManager == null) Debug.LogError("ARFaceManager is not assigned!");
        StartCoroutine(DownloadAndLoadAssetBundle(assetBundleUrl, prefabName));
    }

    IEnumerator DownloadAndLoadAssetBundle(string url, string assetName)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
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
