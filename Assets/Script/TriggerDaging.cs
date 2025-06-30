using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDaging : MonoBehaviour {
    [SerializeField] private GameObject collectButton;
    [SerializeField] private Image collectButtonImage;
    [SerializeField] private Sprite[] gifFrames;
    private float frameDelay = 0.083f;
    private int currentFrame;

    [SerializeField] private GameObject openInfo;
    [SerializeField] private GameObject openInfoButton;
    [SerializeField] private Image openInfoButtonImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private GameObject infoDagingCanvas;
    private int merchantIndex;

    private void OnEnable() {
        StartCoroutine(PlayGIF());
    }

    private void OnDisable() {
        StopCoroutine(PlayGIF());
        StopCoroutine(MerchantDirampokFeedback());
    }

    private void Start() {
        merchantIndex = MerchantManager.Instance.GetCurrentMerchantIndex();
        
        collectButton.SetActive(false);
        collectButton.GetComponent<Button>().onClick.AddListener(OnCollectButtonClick);

        openInfo.SetActive(false);
        infoDagingCanvas.SetActive(false);

        openInfoButton.GetComponent<Button>().onClick.AddListener(() => {
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            
            infoDagingCanvas.SetActive(true);
            PersistentManager.Instance.isUIOpen = true;

            FindObjectOfType<PlayerMovementNew>().StopPlayer();
            // ShopUI.Instance.CloseShopUI();
            TutorialManager.Instance.StartTutorialInfo();
        });

        StartCoroutine(PlayGIF());
    }

    private Dictionary<Collider2D, float> npcTimers = new Dictionary<Collider2D, float>();
    private HashSet<Collider2D> npcProcessed = new HashSet<Collider2D>();
    public float requiredTime = 1.8f;
    public float requiredUtoTime = 3f;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("NPC") && !npcProcessed.Contains(collision)) {
            if (!npcTimers.ContainsKey(collision)) {
                npcTimers[collision] = 0f;  // Mulai timer untuk NPC baru yang masuk
            }

            npcTimers[collision] += Time.deltaTime;  // Tambah waktu untuk NPC ini

            if (npcTimers[collision] >= requiredTime) {
                TambahPenghasilanMerchant(collision);
                npcTimers.Remove(collision);  // Hapus NPC dari daftar setelah logika dijalankan
                npcProcessed.Add(collision);  // Tambahkan NPC ke daftar yang sudah diproses
            }
        }

        if (collision.CompareTag("Uto") && !npcProcessed.Contains(collision)) {
            if (!npcTimers.ContainsKey(collision)) {
                npcTimers[collision] = 0f;  // Mulai timer untuk NPC baru yang masuk
            }

            npcTimers[collision] += Time.deltaTime;  // Tambah waktu untuk NPC ini

            if (npcTimers[collision] >= requiredUtoTime) {
                ResetPenghasilanMerchant(collision);
                StartCoroutine(MerchantDirampokFeedback());
                npcTimers.Remove(collision);  // Hapus NPC dari daftar setelah logika dijalankan
                npcProcessed.Add(collision);
            }
        }
    }

    [Header("--- Feedback Merchant Dirampok ---")]
    [SerializeField] private GameObject merchantDirampokFeedback;
    [SerializeField] private Image merchantDirampokFeedbackImage;
    [SerializeField] private Sprite[] merchantDirampokFrames;
    private float delay_18 = 0.055f; // 18fps
    private IEnumerator MerchantDirampokFeedback()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.coinMinus);
        merchantDirampokFeedback.SetActive(true);
        for (int i = 0; i < merchantDirampokFrames.Length; i++)
        {
            merchantDirampokFeedbackImage.sprite = merchantDirampokFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        merchantDirampokFeedback.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("NPC") || collision.CompareTag("Uto")) {
            npcTimers.Remove(collision);  // Hapus timer jika NPC keluar sebelum 2.5 detik
            npcProcessed.Remove(collision);
        }
    }

    private void TambahPenghasilanMerchant(Collider2D collision) {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (merchantData.stokDagangan > 0) {
            merchantData.penghasilanMerchant += merchantData.hargaDagangan;
            merchantData.stokDagangan -= 0.25f;
            Debug.Log("Penghasilan Daging bertambah " + merchantData.penghasilanMerchant);
        }
        
        if (merchantData.penghasilanMerchant >= (merchantData.hargaDagangan * 5)) {
            collectButton.SetActive(true);
        }
    }

    private void ResetPenghasilanMerchant(Collider2D collision) {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        merchantData.penghasilanMerchant = 0;
        Debug.Log("Penghasilan Daging di-reset oleh Uto!");
        collectButton.SetActive(false);
    }

    private IEnumerator PlayGIF() {
        while (true) {
            collectButtonImage.sprite = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private void OnCollectButtonClick() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        PersistentManager.Instance.UpdateKoin(merchantData.penghasilanMerchant);
        Debug.Log("Koin di Setor!");
        merchantData.penghasilanMerchant = 0;
        Debug.Log("Penghasilan Daging Saat Ini = " + merchantData.penghasilanMerchant);
        collectButton.SetActive(false);
        StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
    }

    public void OnHighlightButton() {
        openInfoButtonImage.sprite = highlightedSprite;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnhighlightButton() {
        openInfoButtonImage.sprite = normalSprite;
    }

    private IEnumerator ScanAstarAfterWait(float waitTime)
    {
        GameObject aStarObject = GameObject.Find("A_Star");
        yield return new WaitForSeconds(waitTime);

        if (aStarObject != null)
        {
            AstarPath pathfinder = aStarObject.GetComponent<AstarPath>(); // Menggunakan AstarPath
            if (pathfinder != null)
            {
                pathfinder.Scan(); // Panggil metode Scan
            }
            else
            {
                Debug.LogWarning("Komponen AstarPath tidak ditemukan pada GameObject A_Star!");
            }
        }
        else
        {
            Debug.LogWarning("GameObject A_Star tidak ditemukan di scene!");
        }
    }
}
