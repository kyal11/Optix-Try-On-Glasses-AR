using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GlassesManager : MonoBehaviour
{
    public ARFaceManager arFaceManager; // Referensi ke ARFaceManager
    public List<GameObject> glassesPrefabs; // Daftar prefab kacamata

    void Start()
    {
        if (arFaceManager == null) Debug.LogError("ARFaceManager is not assigned!");
        if (glassesPrefabs == null || glassesPrefabs.Count == 0) Debug.LogError("GlassesPrefabs list is empty or not assigned!");
    }

    public void ChangeGlasses(GameObject newGlassesPrefab)
    {
        if (arFaceManager != null)
        {
            arFaceManager.facePrefab = newGlassesPrefab;
            // Force update face prefab for existing faces
            foreach (var face in arFaceManager.trackables)
            {
                var arFace = face.GetComponent<ARFace>();
                if (arFace != null)
                {
                    // Remove old glasses
                    foreach (Transform child in arFace.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    // Instantiate new glasses
                    GameObject newGlasses = Instantiate(newGlassesPrefab, arFace.transform);
                    newGlasses.transform.localPosition = Vector3.zero;
                    newGlasses.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }
}
