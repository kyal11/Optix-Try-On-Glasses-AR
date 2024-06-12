using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Canvas MainCanvas; // Referensi ke MainCanvas
    public Canvas PopUpCanvas; // Referensi ke PopUpCanvas
    public Button productDetailButton; // Referensi ke tombol View Detail
    public Button exitButton; // Referensi ke tombol Exit

    void Start()
    {
        // Pastikan MainCanvas aktif dan PopUpCanvas tidak aktif saat memulai
        MainCanvas.gameObject.SetActive(true);
        PopUpCanvas.gameObject.SetActive(false);

        // Menambahkan listener untuk tombol Product Detail
        productDetailButton.onClick.AddListener(ShowPopUp);

        // Menambahkan listener untuk tombol Exit
        exitButton.onClick.AddListener(ClosePopUp);
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