using UnityEngine;
using UnityEngine.UI;

public class TriggerSign : MonoBehaviour {
    [SerializeField] private GameObject windowSign;
    [SerializeField] private Sprite[] windowSignSprites;
    [SerializeField] private Button buttonSignSebelumnya;
    [SerializeField] private Button buttonSignSelanjutnya;
    private int currentSignIndex = 0; // Indeks saat ini untuk sprite yang ditampilkan

    private void Start() {
        buttonSignSebelumnya.onClick.AddListener(PreviousImage);
        buttonSignSelanjutnya.onClick.AddListener(NextImage);
    }

    private void PreviousImage() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        if (currentSignIndex > 0) {
            currentSignIndex--; // Decrement indeks untuk kembali ke gambar sebelumnya
            windowSign.GetComponent<Image>().sprite = windowSignSprites[currentSignIndex];
            UpdateButtonInteractable();
        }
    }

    private void NextImage() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        if (currentSignIndex < windowSignSprites.Length - 1) {
            currentSignIndex++; // Increment indeks untuk menuju ke gambar berikutnya
            windowSign.GetComponent<Image>().sprite = windowSignSprites[currentSignIndex];
            UpdateButtonInteractable();
        }
    }

    private void UpdateButtonInteractable() {
        buttonSignSebelumnya.interactable = currentSignIndex > 0;
        buttonSignSelanjutnya.interactable = currentSignIndex < windowSignSprites.Length - 1;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            windowSign.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            windowSign.SetActive(false);
            currentSignIndex = 0;
        }
    }

    [SerializeField] private Sprite normalSebelumnya;
    [SerializeField] private Sprite highlightedSebelumnya;
    [SerializeField] private Sprite normalSelanjutnya;
    [SerializeField] private Sprite highlightedSelanjutnya;

    public void OnEnterSebelumnya() {
        buttonSignSebelumnya.image.sprite = highlightedSebelumnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSebelumnya() {
        buttonSignSebelumnya.image.sprite = normalSebelumnya;
    }

    public void OnEnterSelanjutnya() {
        buttonSignSelanjutnya.image.sprite = highlightedSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelanjutnya() {
        buttonSignSelanjutnya.image.sprite = normalSelanjutnya;
    }
}
