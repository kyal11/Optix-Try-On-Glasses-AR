using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GlassesManager : MonoBehaviour
{
    public ARFaceManager faceManager;
    public List<GameObject> glassesPrefabs; // Daftar prefab kacamata
    public Button[] glassesButtons; // Array tombol untuk mengganti kacamata

    void Start()
    {
        if (faceManager == null)
        {
            faceManager = FindObjectOfType<ARFaceManager>();
        }

        // Menambahkan listener untuk setiap tombol
        for (int i = 0; i < glassesButtons.Length; i++)
        {
            int index = i; // Local copy untuk digunakan dalam lambda
            glassesButtons[i].onClick.AddListener(() => ChangeGlasses(index));
        }
    }

    public void ChangeGlasses(int index)
    {
        // Validasi indeks kacamata
        if (index < 0 || index >= glassesPrefabs.Count) return;

        // Ganti kacamata pada semua wajah yang terdeteksi
        foreach (var face in faceManager.trackables)
        {
            var arFace = face.GetComponent<ARFace>();
            if (arFace != null)
            {
                // Hapus kacamata lama
                foreach (Transform child in arFace.transform)
                {
                    Destroy(child.gameObject);
                }

                // Tambahkan kacamata baru
                GameObject newGlasses = Instantiate(glassesPrefabs[index], arFace.transform);
                newGlasses.transform.localPosition = Vector3.zero;
                newGlasses.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
