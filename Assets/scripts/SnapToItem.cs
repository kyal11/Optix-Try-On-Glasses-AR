using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnapToItem : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;
    public HorizontalLayoutGroup HLG;
    public TMP_Text NameLabel;
    public string[] ItemNames;
    public GlassesManager glassesManager; // Referensi ke GlassesManager

    bool isSnapped;
    public float snapForce;
    float snapSeed;

    private int currentItemIndex = -1; // Variable to store the current item index

    void Start()
    {
        isSnapped = false;

        // Debugging for null references
        if (scrollRect == null) Debug.LogError("ScrollRect is not assigned!");
        if (contentPanel == null) Debug.LogError("ContentPanel is not assigned!");
        if (sampleListItem == null) Debug.LogError("SampleListItem is not assigned!");
        if (HLG == null) Debug.LogError("HorizontalLayoutGroup is not assigned!");
        if (NameLabel == null) Debug.LogError("NameLabel is not assigned!");
        if (glassesManager == null) Debug.LogError("GlassesManager is not assigned!");
    }

    void Update()
    {
        int currentItem = Mathf.RoundToInt((0 - contentPanel.localPosition.x / (sampleListItem.rect.width + HLG.spacing)));

        if (scrollRect.velocity.magnitude < 200 && !isSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            snapSeed += snapForce * Time.deltaTime;
            contentPanel.localPosition = new Vector3(Mathf.MoveTowards(contentPanel.localPosition.x, 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)), snapSeed), contentPanel.localPosition.y, contentPanel.localPosition.z);
            if (contentPanel.localPosition.x == 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)))
            {
                isSnapped = true;
                currentItemIndex = currentItem; // Update the current item index

                // Update NameLabel with the current item's name
                if (NameLabel != null && ItemNames != null && ItemNames.Length > currentItem)
                {
                    NameLabel.text = ItemNames[currentItem];
                    Debug.Log("Item selected: " + ItemNames[currentItem]);
                }

                // Notify GlassesManager to change the glasses
                if (glassesManager != null && glassesManager.glassesPrefabs != null && glassesManager.glassesPrefabs.Count > currentItem)
                {
                    glassesManager.ChangeGlasses(glassesManager.glassesPrefabs[currentItem]);
                }
            }
        }

        if (scrollRect.velocity.magnitude > 200)
        {
            isSnapped = false;
            snapSeed = 0;
        }
    }

    public int GetCurrentItemIndex()
    {
        return currentItemIndex;
    }
}
