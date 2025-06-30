using UnityEngine;
using UnityEngine.UI;

public class PanduanPanel_3 : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Sprite[] images;

    [Header("Button Sprites States")]
    [SerializeField] private Sprite selanjutnyaNormal;
    [SerializeField] private Sprite selanjutnyaHover;
    [SerializeField] private Sprite sebelumnyaNormal;
    [SerializeField] private Sprite sebelumnyaHover;

    private int currentIndex = 0;

    private void Start()
    {
        // Set gambar awal
        UpdateImage();

        nextButton.onClick.AddListener(ShowNextImage);
        previousButton.onClick.AddListener(ShowPreviousImage);
    }

    private void ShowNextImage()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        if (images.Length == 0) return;

        currentIndex = (currentIndex + 1) % images.Length;
        UpdateImage();
    }

    private void ShowPreviousImage()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        if (images.Length == 0) return; // Jika tidak ada gambar, tidak melakukan apa-apa

        currentIndex = (currentIndex - 1 + images.Length) % images.Length; // Pergi ke gambar sebelumnya
        UpdateImage();
    }

    private void UpdateImage()
    {
        displayImage.sprite = images[currentIndex];

        // Disable previousButton jika berada di index pertama
        previousButton.interactable = currentIndex != 0;

        // Disable nextButton jika berada di index terakhir
        nextButton.interactable = currentIndex != images.Length - 1;
    }

    //

    public void OnEnterSelanjutnya()
    {
        // Cek jika nextButton interactable
        if (!nextButton.interactable) return;

        nextButton.image.sprite = selanjutnyaHover;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnEnterSebelumnya()
    {
        // Cek jika previousButton interactable
        if (!previousButton.interactable) return;

        previousButton.image.sprite = sebelumnyaHover;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelanjutnya()
    {
        nextButton.image.sprite = selanjutnyaNormal;
    }

    public void OnExitSebelumnya()
    {
        previousButton.image.sprite = sebelumnyaNormal;
    }
}
