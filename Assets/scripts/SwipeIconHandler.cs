using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class SwipeIconHandler : MonoBehaviour
{
    [SerializeField]
    private ARFaceManager faceManager;
    [SerializeField]
    private List<GameObject> facePrefabs;
    [SerializeField]
    private List<Image> filterIcons; // Icons representing each filter

    private int currentIndex = 0;

    void Start()
    {
        if (faceManager != null && facePrefabs.Count > 0)
        {
            faceManager.facePrefab = facePrefabs[currentIndex];
            HighlightSelectedIcon();
        }
    }

    public void OnIconSwipe(int direction)
    {
        currentIndex += direction;
        if (currentIndex >= facePrefabs.Count) currentIndex = 0;
        if (currentIndex < 0) currentIndex = facePrefabs.Count - 1;

        faceManager.facePrefab = facePrefabs[currentIndex];
        HighlightSelectedIcon();

        foreach (var face in faceManager.trackables)
        {
            Destroy(face.gameObject);
        }
    }

    private void HighlightSelectedIcon()
    {
        for (int i = 0; i < filterIcons.Count; i++)
        {
            filterIcons[i].color = (i == currentIndex) ? Color.yellow : Color.white; // Highlight selected icon
        }
    }
}
