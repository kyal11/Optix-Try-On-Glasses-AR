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

    public Button likeButton;
    public Image likeButtonImage;
    public Sprite likeSprite;
    public Sprite unlikeSprite;

    private bool isSnapped;
    public float snapForce;
    private float snapSeed;
    private int currentItemIndex = -1;

    // Dictionary to store like status in memory
    private Dictionary<int, bool> likeStatuses = new Dictionary<int, bool>();

    void Start()
    {
        isSnapped = false;

        if (scrollRect == null) Debug.LogError("ScrollRect is not assigned!");
        if (contentPanel == null) Debug.LogError("ContentPanel is not assigned!");
        if (sampleListItem == null) Debug.LogError("SampleListItem is not assigned!");
        if (HLG == null) Debug.LogError("HorizontalLayoutGroup is not assigned!");
        if (NameLabel == null) Debug.LogError("NameLabel is not assigned!");

        if (likeButton != null)
        {
            likeButton.onClick.AddListener(ToggleLikeStatus);
        }

        if (likeButtonImage == null) Debug.LogError("LikeButtonImage is not assigned!");
        if (likeSprite == null) Debug.LogError("LikeSprite is not assigned!");
        if (unlikeSprite == null) Debug.LogError("UnlikeSprite is not assigned!");
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
                currentItemIndex = currentItem;

                if (NameLabel != null && ItemNames != null && ItemNames.Length > currentItem)
                {
                    NameLabel.text = ItemNames[currentItem];
                    Debug.Log("Item selected: " + ItemNames[currentItem]);
                }

                UpdateLikeButton();
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

    private void ToggleLikeStatus()
    {
        Debug.Log("Toogle Like");

            bool isLiked = GetLikeStatus(currentItemIndex);
            isLiked = !isLiked;
            SaveLikeStatus(currentItemIndex, isLiked);
            UpdateLikeButton();
            Debug.Log("Toggled like status for item " + currentItemIndex + ": " + isLiked);
    }

    private void SaveLikeStatus(int itemId, bool isLiked)
    {
        if (likeStatuses.ContainsKey(itemId))
        {
            likeStatuses[itemId] = isLiked;
        }
        else
        {
            likeStatuses.Add(itemId, isLiked);
        }
        Debug.Log("Saved like status for item " + itemId + ": " + isLiked);
    }

    private bool GetLikeStatus(int itemId)
    {
        if (likeStatuses.ContainsKey(itemId))
        {
            Debug.Log("Retrieved like status for item " + itemId + ": " + likeStatuses[itemId]);
            return likeStatuses[itemId];
        }
        Debug.Log("Like status for item " + itemId + " not found, defaulting to false.");
        return false;
    }

    private void UpdateLikeButton()
    {
        Debug.Log("Update Status");
        bool isLiked = GetLikeStatus(currentItemIndex);
        likeButtonImage.sprite = isLiked ? likeSprite : unlikeSprite;

    }
}
