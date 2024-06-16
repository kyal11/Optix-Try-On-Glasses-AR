using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ProductDetailsDisplay : MonoBehaviour
{
    public TextMeshProUGUI productNameText; // Referensi ke TextMeshPro untuk nama produk
    public TextMeshProUGUI productDescriptionText; // Referensi ke TextMeshPro untuk deskripsi produk
    public TextMeshProUGUI productPriceText; // Referensi ke TextMeshPro untuk harga produk

    void Start()
    {
        StartCoroutine(GetProductDetails());
    }

    IEnumerator GetProductDetails()
    {
        string apiUrl = "http://100.97.75.94:8080/products/352"; // URL API produk
        string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ5b2RoYW5hYmloYTJAZ21haWwuY29tIiwidXNlcm5hbWUiOiJ5b2RoYW5hYmloYTJAZ21haWwuY29tIn0.1GYZYS3i7fEyDbd1xcaM1LV7f5UDNYk6rjNzP_Y0ipo"; // Token otorisasi

        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
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
