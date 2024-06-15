using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikeButtonHandler : MonoBehaviour
{
    public Button likeButton;
    public Image uiElementToChangeColor;
    public Color newColor = Color.red;

    private void Start()
    {
        // Pastikan tombol sudah terhubung
        if (likeButton != null)
        {
            likeButton.onClick.AddListener(OnLikeButtonClick);
        }
    }

    private void OnLikeButtonClick()
    {
        if (uiElementToChangeColor != null)
        {
            // Ubah warna elemen UI
            uiElementToChangeColor.color = newColor;
        }
    }
}

