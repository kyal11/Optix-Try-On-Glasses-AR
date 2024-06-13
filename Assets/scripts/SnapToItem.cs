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
    public string[] ItemName;

    bool isSnapped;

    public float snapForce;
    float snapSeed;

    void Start()
    {
        isSnapped = false;

        // Debugging for null references
        if (scrollRect == null) Debug.LogError("ScrollRect is not assigned!");
        if (contentPanel == null) Debug.LogError("ContentPanel is not assigned!");
        if (sampleListItem == null) Debug.LogError("SampleListItem is not assigned!");
        if (HLG == null) Debug.LogError("HorizontalLayoutGroup is not assigned!");
        if (NameLabel == null) Debug.LogError("NameLabel is not assigned!");
    }

    void Update()
    {
        // Null checks to prevent NullReferenceException
        // if (scrollRect == null || contentPanel == null || sampleListItem == null || HLG == null)
        // {
        //     Debug.LogError("One or more references are null in Update!");
        //     return;
        // }

        int currentItem = Mathf.RoundToInt((0 - contentPanel.localPosition.x / (sampleListItem.rect.width + HLG.spacing)));
        Debug.Log(currentItem);

        if (scrollRect.velocity.magnitude < 200 && !isSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            snapSeed += snapForce * Time.deltaTime;
            contentPanel.localPosition = new Vector3(Mathf.MoveTowards(contentPanel.localPosition.x, 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)), snapSeed), contentPanel.localPosition.y, contentPanel.localPosition.z);
            if (contentPanel.localPosition.x == 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)))
            {
                isSnapped = true;
                // Update NameLabel with the current item's name
                if (NameLabel != null && ItemName != null && ItemName.Length > currentItem)
                {
                    NameLabel.text = ItemName[currentItem];
                }
            }
        }

        if (scrollRect.velocity.magnitude > 200)
        {
            isSnapped = false;
            snapSeed = 0;
        }
    }
}
