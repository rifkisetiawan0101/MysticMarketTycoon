using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InvoiceUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI totalKeuanganText;
    [SerializeField] private TextMeshProUGUI jumlahPedagangText;
    [SerializeField] private TextMeshProUGUI setoranPedagangText;
    [SerializeField] private Button buttonMenujuPagi; // Referensi ke button Tutup Pasar
    [SerializeField] private GameObject blokirWorld;

    private void Start() {
        buttonMenujuPagi.onClick.AddListener(OnMenujuPagiClicked);
    }

    private void Update() {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (PersistentManager.Instance.isInvoiceShown == true && !AudioManager.audioManager.hasInvoiceSoundPlayed)
        {
            // Pastikan tidak menjalankan jika berada di scene yang tidak diinginkan
            if (currentSceneName != "InGamePagiFirstCutScene" && currentSceneName != "InGamePagiCutScene")
            {
                AudioManager.audioManager.musicSource.Stop();
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.invoiceSound);
                AudioManager.audioManager.hasInvoiceSoundPlayed = true; // Set flag menjadi true agar tidak dipanggil lagi
            }
        }
    }

    private void OnMenujuPagiClicked() {
        Time.timeScale = 1;
        PersistentManager.Instance.UpdateDayCounter(1);
        PersistentManager.Instance.isNowMalam = false;
        PersistentManager.Instance.isUIOpen = false;
        blokirWorld.SetActive(false);
        gameObject.SetActive(false);

        if (PersistentManager.Instance.dayCounter == 1) {
            SceneManager.LoadScene("InGamePagiFirstCutScene");
        } else if (PersistentManager.Instance.dayCounter > 1) {
            SceneManager.LoadScene("InGamePagiCutScene");
        }
        
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.sceneTransitionSound);
    }

    public void ShowInvoice() {
        PersistentManager.Instance.isUIOpen = true;

        Time.timeScale = 0;

        gameObject.SetActive(true); // Menampilkan InvoiceUI

        float nilaiSetoran = PersistentManager.Instance.dataTotalMerchant * 100;
        setoranPedagangText.text = nilaiSetoran.ToString("N0") + "K";

        dayText.text = PersistentManager.Instance.nightCounter.ToString();
        totalKeuanganText.text = (PersistentManager.Instance.dataKoin + nilaiSetoran).ToString("N0") + "K"; 
        jumlahPedagangText.text = PersistentManager.Instance.dataTotalMerchant.ToString();

        if (PersistentManager.Instance.isInvoiceShown == false) {
            PersistentManager.Instance.UpdateKoin(nilaiSetoran);
        }
    }
    
    [SerializeField] private Image buttonMenujuPagiImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnHighlightButton() {
        buttonMenujuPagiImage.sprite = highlightedSprite;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnhighlightButton() {
        buttonMenujuPagiImage.sprite = normalSprite;
    }
}

