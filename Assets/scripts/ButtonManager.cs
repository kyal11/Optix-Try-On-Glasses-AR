using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Canvas MainCanvas; // Referensi ke MainCanvas
    public Canvas PopUpCanvas; // Referensi ke PopUpCanvas
    public Button productDetailButton; // Referensi ke tombol View Detail
    public Button exitButton; // Referensi ke tombol Exit
    public Button[] glassesButtons; // Array tombol untuk mengganti kacamata

    void Start()
    {
        // Pastikan MainCanvas aktif dan PopUpCanvas tidak aktif saat memulai
        MainCanvas.gameObject.SetActive(true);
        PopUpCanvas.gameObject.SetActive(false);

        // Menambahkan listener untuk tombol Product Detail
        productDetailButton.onClick.AddListener(ShowPopUp);

        // Menambahkan listener untuk tombol Exit
        exitButton.onClick.AddListener(ClosePopUp);

        // Pastikan semua tombol untuk mengganti kacamata terhubung dengan fungsi yang sesuai
        GlassesManager glassesManager = FindObjectOfType<GlassesManager>();
        if (glassesManager != null)
        {
            glassesManager.glassesButtons = glassesButtons;
        }
    }

    public void ShowPopUp()
    {
        // Menampilkan PopUpCanvas
        PopUpCanvas.gameObject.SetActive(true);
    }

    public void ClosePopUp()
    {
        // Menyembunyikan PopUpCanvas
        PopUpCanvas.gameObject.SetActive(false);
    }
}
