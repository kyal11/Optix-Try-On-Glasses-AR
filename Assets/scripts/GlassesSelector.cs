using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlassesSelector : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect; // Referensi ke ScrollRect
    [SerializeField] private Canvas PopUpCanvas; // Referensi ke PopUpCanvas
    [SerializeField] private Image popUpImage; // Referensi ke gambar di PopUpCanvas
    [SerializeField] private Sprite[] glassesSprites; // Array gambar kacamata untuk PopUpCanvas
    [SerializeField] private GameObject[] glassesObjects; // Array objek kacamata 3D
    [SerializeField] private Button likeButton; // Referensi ke tombol like
    [SerializeField] private Image uiElementToChangeColor; // Referensi ke elemen UI yang berubah warna
    [SerializeField] private Color likeColor = Color.red; // Warna ketika like
    [SerializeField] private Color unlikeColor = Color.white; // Warna ketika unlike

    private int currentIndex = 0;
    private bool[] likeStatuses; // Array untuk menyimpan status like setiap objek

    void Start()
    {
        likeStatuses = new bool[glassesObjects.Length]; // Inisialisasi array status like
        UpdateGlasses();
        if (likeButton != null)
        {
            likeButton.onClick.AddListener(OnLikeButtonClick);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Hitung jarak setiap item ke tengah ScrollView
        float[] distances = new float[glassesSprites.Length];
        for (int i = 0; i < glassesSprites.Length; i++)
        {
            Transform item = scrollRect.content.GetChild(i);
            distances[i] = Mathf.Abs(scrollRect.viewport.transform.position.x - item.position.x);
        }

        // Temukan item yang paling dekat dengan tengah
        currentIndex = 0;
        float minDistance = distances[0];
        for (int i = 1; i < distances.Length; i++)
        {
            if (distances[i] < minDistance)
            {
                minDistance = distances[i];
                currentIndex = i;
            }
        }

        UpdateGlasses();
    }

    void UpdateGlasses()
    {
        // Perbarui objek kacamata yang ditampilkan
        for (int i = 0; i < glassesObjects.Length; i++)
        {
            glassesObjects[i].SetActive(i == currentIndex);
        }

        // Perbarui gambar di PopUpCanvas
        popUpImage.sprite = glassesSprites[currentIndex];

        // Perbarui status like pada elemen UI
        if (likeStatuses[currentIndex])
        {
            uiElementToChangeColor.color = likeColor;
        }
        else
        {
            uiElementToChangeColor.color = unlikeColor;
        }
    }

    private void OnLikeButtonClick()
    {
        // Toggle status like
        likeStatuses[currentIndex] = !likeStatuses[currentIndex];

        // Perbarui warna elemen UI
        if (likeStatuses[currentIndex])
        {
            uiElementToChangeColor.color = likeColor;
        }
        else
        {
            uiElementToChangeColor.color = unlikeColor;
        }
    }
}
