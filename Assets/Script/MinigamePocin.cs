using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinigamePocin : MonoBehaviour
{   
    [SerializeField] private CanvasGroup uI;
    [SerializeField] private GameObject buttonQuest;
    [SerializeField] private GameObject canvasPocin;
    [SerializeField] private Image pocinDialog;
    [SerializeField] private Sprite[] pocinDialogSprites;

    private int dialogIndex;
    [SerializeField] private GameObject blokirMinigame;
    [SerializeField] private GameObject buttonSelanjutnya;
    [SerializeField] private GameObject buttonMulai;
    [SerializeField] private GameObject buttonSelesai;

    public GameObject bakulGlow;
    [SerializeField] private GameObject bakulNormal;
    [SerializeField] private GameObject[] items;

    [SerializeField] private bool isSempurna;
    [SerializeField] private bool isSkillIsu;

    private int itemMasukCount = 0;
    private int totalItems;

    private void Start()
    {
        pocinGameGFX.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        foreach (GameObject item in items)
        {
            totalItems = items.Length; // Menyimpan jumlah total item

            if (item.GetComponent<DragDropItem>() == null)
            {
                item.AddComponent<DragDropItem>();
            }
        }

        StartCoroutine(PocinGIF());
        StartCoroutine(PlaySempurna());
        StartCoroutine(PlaySkillIsu());

        buttonQuest.SetActive(false);
        canvasPocin.SetActive(false);
        countdownMinigame.SetActive(false);
        countdownStart.SetActive(false);
        buttonMulai.SetActive(false);
        buttonSelanjutnya.SetActive(false);

        buttonQuest.GetComponent<Button>().onClick.AddListener(() => {
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            canvasPocin.SetActive(true);
            buttonSelanjutnya.SetActive(true);
            SetUIVisibility(false);
            PersistentManager.Instance.isPlayTimer = false;

            bakulNormal.SetActive(true);
            foreach (GameObject item in items)
            {
                item.SetActive(true);
            }
        });

        buttonSelanjutnya.GetComponent<Button>().onClick.AddListener(() => {
            dialogIndex++;
            pocinDialog.GetComponent<Image>().sprite = pocinDialogSprites[dialogIndex];
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            if (pocinDialogSprites[1])
            {
                buttonSelanjutnya.SetActive(false);
                buttonMulai.SetActive(true);
            }
        });

        buttonMulai.GetComponent<Button>().onClick.AddListener(() => {
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            blokirMinigame.SetActive(false);
            buttonMulai.SetActive(false);
            StartCoroutine(PlayCountdownStart());
            StartCoroutine(PlayCountdownMinigame());
        });

        buttonSelesai.GetComponent<Button>().onClick.AddListener(() => {
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            StartCoroutine(DestroyMinigameAfterDelay());
            
            if (isSempurna == true) {
                PersistentManager.Instance.UpdateBatuAkik(1);
                StartCoroutine(FeedbackManager.instance.PlayCollectBatuAkik());
            }

            SetUIVisibility(true);
            countdownMinigame.SetActive(false);
            PersistentManager.Instance.isPlayTimer = true;

            bakulNormal.SetActive(false);
            foreach (GameObject item in items)
            {
                item.SetActive(false);
            }
        });
    }

    [SerializeField] private Timer timer;

    private void OnEnable() {
        timer.OnPocinActive += HandlePocinActive;
        timer.OnPocinDeactive += HandlePocinDeactive;
    }

    private void HandlePocinActive() {
        pocinGameGFX.SetActive(true);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void HandlePocinDeactive() {
        if (isSempurna == false || isSkillIsu == false) {
            pocinGameGFX.SetActive(false);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonQuest.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonQuest.SetActive(false);
        }
    }

    [Header("Animation")]
    [SerializeField] private float delay_18 = 0.055f; // 18fps
    float audioPlayTime = 0f;
    [SerializeField] private GameObject pocinGameGFX;
    [SerializeField] private Sprite[] pocinGameFrames;
    private int pocinFrame;
    private IEnumerator PocinGIF() {
        while (true) {
            pocinGameGFX.GetComponent<SpriteRenderer>().sprite = pocinGameFrames[pocinFrame];
            pocinFrame = (pocinFrame + 1) % pocinGameFrames.Length;
            yield return new WaitForSeconds(delay_18);
        }
    }

    [SerializeField] private GameObject countdownStart;
    [SerializeField] private Sprite[] countdownStartFrames;
    private IEnumerator PlayCountdownStart()
    {
        countdownStart.SetActive(true);
        for (int i = 0; i < countdownStartFrames.Length; i++)
        {
            countdownStart.GetComponent<Image>().sprite = countdownStartFrames[i];

            if (Time.time - audioPlayTime >= 1f) {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.countDownSound);
                audioPlayTime = Time.time; // Perbarui waktu audio terakhir diputar
            }

            yield return new WaitForSeconds(delay_18);
        }
        countdownStart.SetActive(false);
    }

    [SerializeField] private GameObject countdownMinigame;
    [SerializeField] private Sprite[] countdownMinigameFrames;
    private IEnumerator PlayCountdownMinigame()
    {
        yield return new WaitForSeconds(3f);
        countdownMinigame.SetActive(true);
        for (int i = 0; i < countdownMinigameFrames.Length; i++)
        {
            countdownMinigame.GetComponent<Image>().sprite = countdownMinigameFrames[i];

            if (Time.time - audioPlayTime >= 1f) {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.countDownSound);
                audioPlayTime = Time.time; // Perbarui waktu audio terakhir diputar
            }

            if (isSempurna == true || isSkillIsu == true) {
                break;
            }

            yield return new WaitForSeconds(delay_18);
        }

        if (itemMasukCount == totalItems)
        {
            isSempurna = true;
            pocinDialog.GetComponent<Image>().sprite = pocinDialogSprites[2];
        } else {
            isSkillIsu = true;
            pocinDialog.GetComponent<Image>().sprite = pocinDialogSprites[3];
        }
    }

    [SerializeField] private GameObject sempurna;
    [SerializeField] private Sprite[] sempurnaFrames;
    private IEnumerator PlaySempurna()
    {
        sempurna.SetActive(false);
        yield return new WaitUntil(() => isSempurna == true);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.winMiniGameSound);
        sempurna.SetActive(true);
        buttonSelesai.SetActive(true);
        for (int i = 0; i < sempurnaFrames.Length; i++)
        {
            sempurna.GetComponent<Image>().sprite = sempurnaFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        sempurna.SetActive(false);
    }

    [SerializeField] private GameObject skillIsu;
    [SerializeField] private Sprite[] skillIsuFrames;
    private IEnumerator PlaySkillIsu()
    {
        skillIsu.SetActive(false);
        yield return new WaitUntil(() => isSkillIsu == true);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.loseMiniGameSound);
        skillIsu.SetActive(true);
        buttonSelesai.SetActive(true);
        for (int i = 0; i < skillIsuFrames.Length; i++) {
            skillIsu.GetComponent<Image>().sprite = skillIsuFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
    }

    public void ItemMasukKeBakul()
    {
        itemMasukCount++;

        if (itemMasukCount == totalItems)
        {
            Debug.Log("Semua item berhasil masuk ke bakul");
            isSempurna = true;
        }
    }

    private IEnumerator DestroyMinigameAfterDelay()
    {
        canvasPocin.SetActive(false);
        buttonQuest.SetActive(false);
        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);
    }

    [Header("Button States")]
    [SerializeField] private Sprite normalQuest;
    [SerializeField] private Sprite highlightedQuest;
    [SerializeField] private Sprite normalSelanjutnya;
    [SerializeField] private Sprite highlightedSelanjutnya;
    [SerializeField] private Sprite normalMulai;
    [SerializeField] private Sprite highlightedMulai;
    [SerializeField] private Sprite normalSelesai;
    [SerializeField] private Sprite highlightedSelesai;

    public void OnEnterQuest()
    {
        buttonQuest.GetComponent<Image>().sprite = highlightedQuest;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitQuest()
    {
        buttonQuest.GetComponent<Image>().sprite = normalQuest;
    }

    public void OnEnterSelanjutnya()
    {
        buttonSelanjutnya.GetComponent<Image>().sprite = highlightedSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelanjutnya()
    {
        buttonSelanjutnya.GetComponent<Image>().sprite = normalSelanjutnya;
    }

    public void OnEnterMulai()
    {
        buttonMulai.GetComponent<Image>().sprite = highlightedMulai;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMulai()
    {
        buttonMulai.GetComponent<Image>().sprite = normalMulai;
    }

    public void OnEnterSelesai()
    {
        buttonSelesai.GetComponent<Image>().sprite = highlightedSelesai;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelesai()
    {
        buttonSelesai.GetComponent<Image>().sprite = normalSelesai;
    }

    private void SetUIVisibility(bool isVisible)
    {
        if (isVisible == true)
        {
            uI.alpha = 1f;             // Atur alpha menjadi 1 untuk membuatnya terlihat
            uI.interactable = true;    // Membuat elemen UI dapat diinteraksi
            uI.blocksRaycasts = true;  // Membuat elemen UI dapat memblokir input
        }
        else
        {
            uI.alpha = 0f;             // Atur alpha menjadi 0 untuk menyembunyikan UI
            uI.interactable = false;   // Nonaktifkan interaktivitas
            uI.blocksRaycasts = false; // Nonaktifkan pemblokiran input
        }
    }

    public Texture2D customCursor; // Tekstur untuk kursor yang baru
    public Texture2D normalCursor;
    public Vector2 hotSpot = Vector2.zero; // Titik fokus kursor
    public CursorMode cursorMode = CursorMode.Auto;

    public void OnPointerEnter() {
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnPointerExit() {
        Cursor.SetCursor(normalCursor, hotSpot, cursorMode);
    }
}