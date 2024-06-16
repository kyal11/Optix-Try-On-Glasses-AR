using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikeButtonHandler : MonoBehaviour
{
    public Button likeButton;
    public Image uiElementToChangeColor;
    public Color newColor = Color.red;

    private Color originalColor;
    private bool isLiked = false;

    private void Start()
    {
        // Pastikan tombol sudah terhubung
        if (likeButton != null)
        {
            likeButton.onClick.AddListener(OnLikeButtonClick);
        }

        // Simpan warna asli elemen UI
        if (uiElementToChangeColor != null)
        {
            originalColor = uiElementToChangeColor.color;
        }
    }

    private void OnLikeButtonClick()
    {
        if (uiElementToChangeColor != null)
        {
            if (isLiked)
            {
                // Kembalikan warna asli
                uiElementToChangeColor.color = originalColor;
                isLiked = false;
            }
            else
            {
                // Ubah ke warna baru
                uiElementToChangeColor.color = newColor;
                isLiked = true;
            }
        }
    }
}
