using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ARGlassesManager : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect; // Referensi ke ScrollRect
    [SerializeField] private Canvas PopUpCanvas; // Referensi ke PopUpCanvas
    [SerializeField] private Image popUpImage; // Referensi ke gambar di PopUpCanvas
    [SerializeField] private Sprite[] glassesSprites; // Array gambar kacamata untuk PopUpCanvas
    [SerializeField] private GameObject[] glassesPrefabs; // Array prefab kacamata 3D
    [SerializeField] private ARFaceManager arFaceManager; // Referensi ke ARFaceManager

    private int currentIndex = 0;
    private Dictionary<TrackableId, GameObject> instantiatedGlasses = new Dictionary<TrackableId, GameObject>();

    void Start()
    {
        arFaceManager.facesChanged += OnFacesChanged;
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
        // Perbarui gambar di PopUpCanvas
        popUpImage.sprite = glassesSprites[currentIndex];

        // Perbarui objek kacamata yang ditampilkan pada wajah yang terdeteksi
        foreach (var face in arFaceManager.trackables)
        {
            UpdateFaceGlasses(face);
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        // Hapus kacamata dari wajah yang tidak lagi terdeteksi
        foreach (var removedFace in eventArgs.removed)
        {
            if (instantiatedGlasses.ContainsKey(removedFace.trackableId))
            {
                Destroy(instantiatedGlasses[removedFace.trackableId]);
                instantiatedGlasses.Remove(removedFace.trackableId);
            }
        }

        // Tambahkan kacamata ke wajah yang baru terdeteksi
        foreach (var addedFace in eventArgs.added)
        {
            UpdateFaceGlasses(addedFace);
        }

        // Perbarui kacamata pada wajah yang diperbarui
        foreach (var updatedFace in eventArgs.updated)
        {
            UpdateFaceGlasses(updatedFace);
        }
    }

    void UpdateFaceGlasses(ARFace face)
    {
        if (instantiatedGlasses.ContainsKey(face.trackableId))
        {
            Destroy(instantiatedGlasses[face.trackableId]);
        }

        var newGlasses = Instantiate(glassesPrefabs[currentIndex], face.transform);
        instantiatedGlasses[face.trackableId] = newGlasses;
    }
}
