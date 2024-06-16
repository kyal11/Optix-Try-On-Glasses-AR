using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Canvas MainCanvas; // Referensi ke MainCanvas
    public Canvas PopUpCanvas; // Referensi ke PopUpCanvas
    public Button productDetailButton; // Referensi ke tombol View Detail
    public Button exitButton; // Referensi ke tombol Exit
    public RawImage productRawImage; // Referensi ke RawImage pada PopUpCanvas
    public SnapToItem snapToItem; // Referensi ke SnapToItem script
    public Texture2D[] glassesTextures; // Array of Textures for glasses

    void Start()
    {
        // Pastikan MainCanvas aktif dan PopUpCanvas tidak aktif saat memulai
        MainCanvas.gameObject.SetActive(true);
        PopUpCanvas.gameObject.SetActive(false);

        // Menambahkan listener untuk tombol Product Detail
        productDetailButton.onClick.AddListener(ShowPopUp);

        // Menambahkan listener untuk tombol Exit
        exitButton.onClick.AddListener(ClosePopUp);

        // Debugging for null references
        if (productRawImage == null) Debug.LogError("ProductRawImage is not assigned!");
        if (snapToItem == null) Debug.LogError("SnapToItem is not assigned!");
        if (glassesTextures == null || glassesTextures.Length == 0) Debug.LogError("GlassesTextures array is empty or not assigned!");
    }

    public void ShowPopUp()
    {
        // Menampilkan PopUpCanvas
        PopUpCanvas.gameObject.SetActive(true);

        // Update product image based on the selected item
        int currentItem = snapToItem.GetCurrentItemIndex();
        if (currentItem >= 0 && currentItem < glassesTextures.Length)
        {
            productRawImage.texture = glassesTextures[currentItem];
            Debug.Log("Pop-up image updated to: " + glassesTextures[currentItem].name);
        }
        else
        {
            Debug.LogError("Invalid item index or texture not found!");
        }
    }

    public void ClosePopUp()
    {
        // Menyembunyikan PopUpCanvas
        PopUpCanvas.gameObject.SetActive(false);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
