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

    private int currentIndex = 0;

    void Start()
    {
        UpdateGlasses();
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
    }
}
