using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PrefabLoader : MonoBehaviour
{
    private string apiUrl = "http://100.97.75.94:8080/products/352";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ5b2RoYW5hYmloYTJAZ21haWwuY29tIiwidXNlcm5hbWUiOiJ5b2RoYW5hYmloYTJAZ21haWwuY29tIn0.1GYZYS3i7fEyDbd1xcaM1LV7f5UDNYk6rjNzP_Y0ipo";
    public TextMeshProUGUI productNameText; // Referensi ke TextMeshPro untuk nama produk
    public TextMeshProUGUI productDescriptionText; // Referensi ke TextMeshPro untuk deskripsi produk
    public TextMeshProUGUI productPriceText; // Referensi ke TextMeshPro untuk harga produk

    void Start()
    {
        StartCoroutine(GetProductDetails());
    }

    IEnumerator GetProductDetails()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        www.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to get product details: " + www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log(jsonResponse);
            ProductData productData = JsonUtility.FromJson<ProductData>(jsonResponse);

            if (productData != null && productData.results != null)
            {
                productNameText.text = productData.results.title;
                productDescriptionText.text = productData.results.description;
                productPriceText.text = "Price: $" + productData.results.price.ToString();

                Debug.Log("Product Name: " + productData.results.title);
                Debug.Log("Product Description: " + productData.results.description);
                Debug.Log("Product Price: $" + productData.results.price.ToString());

               // string prefabUrl = "http://100.97.75.94:8080" + productData.results.imageurl;
                //StartCoroutine(DownloadAndInstantiatePrefab(prefabUrl));
            }
            else
            {
                Debug.LogError("Failed to parse product data.");
            }
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

    [System.Serializable]
    public class ProductData
    {
        public string status;
        public Results results;
    }

    [System.Serializable]
    public class Results
    {
        public int id;
        public string title;
        public string description;
        public float price;
        public string imageurl;
    }
}
