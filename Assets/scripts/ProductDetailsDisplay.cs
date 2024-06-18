using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProductDetailsDisplay : MonoBehaviour
{
    public TextMeshProUGUI productNameText; // Referensi ke TextMeshPro untuk nama produk
    public TextMeshProUGUI productDescriptionText; // Referensi ke TextMeshPro untuk deskripsi produk
    public TextMeshProUGUI productPriceText; // Referensi ke TextMeshPro untuk harga produk
    public Image productImage; // Referensi ke UI Image untuk menampilkan gambar produk
    public Button likeButton; // Referensi ke Button untuk like/unlike
    public Image likeButtonImage; // Referensi ke Image komponen di dalam tombol like
    public Sprite likeSprite; // Sprite untuk like
    public Sprite unlikeSprite; // Sprite untuk unlike
    private string apiUrl = "http://100.97.75.94:8080";
    private string authToken = "";
    private int productId = -1;
    private bool isLiked = false;
    private int likeId = -1;
    private int userId = -1; // ID pengguna (dapat diperoleh dari autentikasi atau data intent)

    void Start()
    {
        GetIntentData();
        StartCoroutine(GetProductDetails());
        likeButton.onClick.AddListener(ToggleLikeStatus);
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
                    userId = intent.Call<int>("getIntExtra", "userId", -1);
                    authToken = intent.Call<string>("getStringExtra", "authToken");
                    apiUrl = intent.Call<string>("getStringExtra", "apiUrl");

                    Debug.Log("ProductId from Intent: " + productId);
                    Debug.Log("AuthToken from Intent: " + authToken);
                    Debug.Log("ApiUrl from Intent: " + apiUrl);
                    Debug.Log("UserId from Intent: " + userId);
                }
            }
        }
    }

    IEnumerator GetProductDetails()
    {
        string productDetailsUrl = apiUrl + "/products/" + productId;

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
                productPriceText.text = "Rp. " + productData.results.price.ToString();

                string imageUrl = apiUrl + productData.results.imageurl;
                StartCoroutine(DownloadImage(imageUrl));

                // Update like status
                if (productData.results.likes != null && productData.results.likes.Count > 0)
                {
                    isLiked = true;
                    likeId = productData.results.likes[0].id;
                }
                else
                {
                    isLiked = false;
                    likeId = -1;
                }
                UpdateLikeButtonSprite();
            }
            else
            {
                Debug.LogError("Failed to parse product data.");
            }
        }
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        www.SetRequestHeader("Authorization", "Bearer " + authToken);

        Debug.Log("Downloading image from URL: " + imageUrl);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download image: " + www.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

            if (texture == null)
            {
                Debug.LogError("Texture is null after download.");
                yield break;
            }

            Debug.Log("Image downloaded successfully: " + texture.width + "x" + texture.height);

            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(texture, rect, pivot);

            productImage.sprite = sprite;
        }
    }


    void ToggleLikeStatus()
    {
        StartCoroutine(SendLikeRequest(!isLiked));
    }

    IEnumerator SendLikeRequest(bool like)
    {
        if (like)
        {
            // Sending like request (POST)
            string likeUrl = apiUrl + "/likes";
            UnityWebRequest www = new UnityWebRequest(likeUrl, "POST");
            www.SetRequestHeader("Authorization", "Bearer " + authToken);
            www.SetRequestHeader("Content-Type", "application/json");

            LikeRequestBody body = new LikeRequestBody { product_id = productId, user_id = userId };
            string jsonBody = JsonUtility.ToJson(body);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to like the product: " + www.error);
            }
            else
            {
                isLiked = true;
                LikeResponse response = JsonUtility.FromJson<LikeResponse>(www.downloadHandler.text);
                likeId = response.results.id;
                UpdateLikeButtonSprite();
                Debug.Log("Liked the product successfully.");
            }
        }
        else
        {
            // Sending unlike request (DELETE)
            if (likeId == -1)
            {
                Debug.LogError("Cannot unlike the product because it was not liked.");
                yield break;
            }

            string unlikeUrl = apiUrl + "/likes/" + likeId;
            UnityWebRequest www = UnityWebRequest.Delete(unlikeUrl);
            www.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to unlike the product: " + www.error);
            }
            else
            {
                isLiked = false;
                likeId = -1;
                UpdateLikeButtonSprite();
                Debug.Log("Unliked the product successfully.");
            }
        }
    }

    void UpdateLikeButtonSprite()
    {
        likeButtonImage.sprite = isLiked ? likeSprite : unlikeSprite;
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
        public List<Like> likes;
    }

    [System.Serializable]
    public class Like
    {
        public int id;
        public Product product;
        public User user;
    }

    [System.Serializable]
    public class Product
    {
        public int id;
        public string title;
        public string description;
        public float price;
        public string imageurl;
    }

    [System.Serializable]
    public class User
    {
        public int id;
        public string email;
        public string name;
        public string phone;
        public string imageurl;
        public string role;
        public string date_birth;
        public string gender;
        public string at_created;
        public string at_updated;
    }

    [System.Serializable]
    public class LikeRequestBody
    {
        public int product_id;
        public int user_id;
    }

    [System.Serializable]
    public class LikeResponse
    {
        public Results results;
    }
}
