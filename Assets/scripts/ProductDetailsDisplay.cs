using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ProductDetailsDisplay : MonoBehaviour
{
    public TextMeshProUGUI productNameText; // Referensi ke TextMeshPro untuk nama produk
    public TextMeshProUGUI productDescriptionText; // Referensi ke TextMeshPro untuk deskripsi produk
    public TextMeshProUGUI productPriceText; // Referensi ke TextMeshPro untuk harga produk

    private string apiUrl;
    private string authToken;
    private int productId;

    void Start()
    {
        GetIntentData();
        StartCoroutine(GetProductDetails());
    }

    void GetIntentData()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent"))
                {
                    productId = intent.Call<int>("getIntExtra", "productId", -1);
                    authToken = intent.Call<string>("getStringExtra", "authToken");
                    apiUrl = intent.Call<string>("getStringExtra", "apiUrl");

                    Debug.Log("ProductId from Intent: " + productId);
                    Debug.Log("AuthToken from Intent: " + authToken);
                    Debug.Log("ApiUrl from Intent: " + apiUrl);
                }
            }
        }
    }

    IEnumerator GetProductDetails()
    {
        string productDetailsUrl = apiUrl + "/products/" + productId; // URL API produk dengan apiUrl dan productId

        UnityWebRequest www = UnityWebRequest.Get(productDetailsUrl);
        www.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get product details: " + www.error);
        }
        else
        {
            // Parsing JSON response
            string jsonResponse = www.downloadHandler.text;
            ProductData productData = JsonUtility.FromJson<ProductData>(jsonResponse);
            Debug.Log(productData);

            // Menampilkan data ke TextMeshPro
            if (productData != null)
            {
                productNameText.text = productData.results.title;
                productDescriptionText.text = productData.results.description;
                productPriceText.text = "Price: $" + productData.results.price.ToString();

                // Debug untuk memeriksa data yang diterima
                Debug.Log("Product Name: " + productData.results.title);
                Debug.Log("Product Description: " + productData.results.description);
                Debug.Log("Product Price: $" + productData.results.price.ToString());
            }
            else
            {
                Debug.LogError("Failed to parse product data.");
            }
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
    }
}
