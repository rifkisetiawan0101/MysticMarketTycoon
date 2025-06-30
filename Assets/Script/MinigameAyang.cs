using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinigameAyang : MonoBehaviour {
    [SerializeField] private GameObject buttonQuest;
    [SerializeField] private GameObject canvasAyang;
    [SerializeField] private Image ayangDialog;
    [SerializeField] private Sprite[] ayangDialogSprites;
    private int dialogIndex;
    [SerializeField] private GameObject blokirMinigame;
    [SerializeField] private GameObject buttonSelanjutnya;
    [SerializeField] private GameObject buttonMulai;
    [SerializeField] private GameObject buttonSelesai;

    [SerializeField] private bool isSempurna;
    [SerializeField] private bool isSkillIsu;

    private void Start() {
        ayangGameGfx.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        buttonQuest.SetActive(false);
        canvasAyang.SetActive(false);
        countdownMinigame.SetActive(false);
        countdownStart.SetActive(false);
        buttonMulai.SetActive(false);
        SetItemsInteractable(false);

        StartCoroutine(AyangGIF());
        StartCoroutine(PlaySempurna());
        StartCoroutine(PlaySkillIsu());

        buttonQuest.GetComponent<Button>().onClick.AddListener (() => {
            canvasAyang.SetActive(true);
            SetUIVisibility(false);
            PersistentManager.Instance.isPlayTimer = false;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonSelanjutnya.GetComponent<Button>().onClick.AddListener (() => {
            dialogIndex++;
            ayangDialog.GetComponent<Image>().sprite = ayangDialogSprites[dialogIndex];
            if (dialogIndex == 1) {
                buttonSelanjutnya.SetActive(false);
                buttonMulai.SetActive(true);
            }
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonMulai.GetComponent<Button>().onClick.AddListener (() => {
            // blokirMinigame.SetActive(false); // pindah ke PlayCountdownMinigame
            StartCoroutine(PlayCountdownStart());
            StartCoroutine(PlayCountdownMinigame());
            SetItemsInteractable(true);
            buttonMulai.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonDaging_1.onClick.AddListener(() => {
            ChooseMechanics(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });
        buttonAyam_1.onClick.AddListener(() => {
            ChooseMechanics(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });
        buttonDaging_2.onClick.AddListener(() => {
            ChooseMechanics(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });
        buttonKambing.onClick.AddListener(() => {
            ChooseMechanics(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });
        buttonCumi.onClick.AddListener(() => {
            ChooseMechanics(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });
        buttonIkan.onClick.AddListener(() => {
            ChooseMechanics(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });
        buttonAyam_2.onClick.AddListener(() => {
            ChooseMechanics(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);
        });

        buttonSelesai.GetComponent<Button>().onClick.AddListener (() => {
            SetUIVisibility(true);
            StartCoroutine(DestroyMinigameAfterDelay());

            if (isSempurna == true) {
                PersistentManager.Instance.UpdateBatuAkik(1);
                StartCoroutine(FeedbackManager.instance.PlayCollectBatuAkik());
            }
            
            PersistentManager.Instance.isPlayTimer = true;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });
    }

    [SerializeField] private Timer timer;

    private void OnEnable() {
        timer.OnAyangActive += HandleAyangActive;
    }

    private void HandleAyangActive() {
        ayangGameGfx.SetActive(true);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            buttonQuest.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            buttonQuest.SetActive(false);
        }
    }

    [Header("Mechanics")]
    [SerializeField] private CanvasGroup UI;
    [SerializeField] private Button buttonDaging_1;
    [SerializeField] private Button buttonAyam_1;
    [SerializeField] private Button buttonDaging_2;
    [SerializeField] private Button buttonKambing;
    [SerializeField] private Button buttonCumi;
    [SerializeField] private Button buttonIkan;
    [SerializeField] private Button buttonAyam_2;

    private void SetItemsInteractable(bool interactable) {
        buttonDaging_1.interactable = interactable;
        buttonAyam_1.interactable = interactable;
        buttonDaging_2.interactable = interactable;
        buttonKambing.interactable = interactable;
        buttonCumi.interactable = interactable;
        buttonIkan.interactable = interactable;
        buttonAyam_2.interactable = interactable;
    }

    private void ChooseMechanics(bool result) {
        if (result == true) {
            isSempurna = true;
            countdownMinigame.SetActive(false);
            buttonSelesai.SetActive(true);
            ayangDialog.GetComponent<Image>().sprite = ayangDialogSprites[2];
        } else {
            isSkillIsu = true;
            countdownMinigame.SetActive(false);
            buttonSelesai.SetActive(true);
            ayangDialog.GetComponent<Image>().sprite = ayangDialogSprites[3];
        }
    }

    private void SetUIVisibility(bool isVisible) {
        if (isVisible == true) {
            UI.alpha = 1f;             // Atur alpha menjadi 1 untuk membuatnya terlihat
            UI.interactable = true;    // Membuat elemen UI dapat diinteraksi
            UI.blocksRaycasts = true;  // Membuat elemen UI dapat memblokir input
        }
        else {
            UI.alpha = 0f;             // Atur alpha menjadi 0 untuk menyembunyikan UI
            UI.interactable = false;   // Nonaktifkan interaktivitas
            UI.blocksRaycasts = false; // Nonaktifkan pemblokiran input
        }
    }

    [Header("Animation")]
    [SerializeField] private float delay_18 = 0.055f; // 18fps
    float audioPlayTime = 0f;
    [SerializeField] private GameObject ayangGameGfx;
    [SerializeField] private Sprite[] ayangGameFrames;
    private int ayangFrame;
    private IEnumerator AyangGIF() {
        while (true) {
            ayangGameGfx.GetComponent<SpriteRenderer>().sprite = ayangGameFrames[ayangFrame];
            ayangFrame = (ayangFrame + 1) % ayangGameFrames.Length;
            yield return new WaitForSeconds(delay_18);
        }
    }

    [SerializeField] private GameObject countdownStart;
    [SerializeField] private Sprite[] countdownStartFrames;
    private IEnumerator PlayCountdownStart() {
        countdownStart.SetActive(true);
        for (int i = 0; i < countdownStartFrames.Length; i++) {
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
    private IEnumerator PlayCountdownMinigame() {
        yield return new WaitForSeconds (3f);
        blokirMinigame.SetActive(false);
        countdownMinigame.SetActive(true);
        for (int i = 0; i < countdownMinigameFrames.Length; i++) {
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
        countdownMinigame.SetActive(false);
    }

    [SerializeField] private GameObject sempurna;
    [SerializeField] private Sprite[] sempurnaFrames;
    private IEnumerator PlaySempurna() {
        sempurna.SetActive(false);
        yield return new WaitUntil (() => isSempurna == true);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.winMiniGameSound);
        sempurna.SetActive(true);
        for (int i = 0; i < sempurnaFrames.Length; i++) {
            sempurna.GetComponent<Image>().sprite = sempurnaFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
    }

    [SerializeField] private GameObject skillIsu;
    [SerializeField] private Sprite[] skillIsuFrames;
    private IEnumerator PlaySkillIsu() {
        skillIsu.SetActive(false);
        yield return new WaitUntil (() => isSkillIsu == true);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.loseMiniGameSound);
        skillIsu.SetActive(true);
        for (int i = 0; i < skillIsuFrames.Length; i++) {
            skillIsu.GetComponent<Image>().sprite = skillIsuFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
    }

    private IEnumerator DestroyMinigameAfterDelay() {
        canvasAyang.SetActive(false);
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
    
    public void OnEnterQuest() {
        buttonQuest.GetComponent<Image>().sprite = highlightedQuest;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitQuest() {
        buttonQuest.GetComponent<Image>().sprite = normalQuest;
    }

    public void OnEnterSelanjutnya() {
        buttonSelanjutnya.GetComponent<Image>().sprite = highlightedSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelanjutnya() {
        buttonSelanjutnya.GetComponent<Image>().sprite = normalSelanjutnya;
    }

    public void OnEnterMulai() {
        buttonMulai.GetComponent<Image>().sprite = highlightedMulai;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMulai() {
        buttonMulai.GetComponent<Image>().sprite = normalMulai;
    }

    public void OnEnterSelesai() {
        buttonSelesai.GetComponent<Image>().sprite = highlightedSelesai;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelesai() {
        buttonSelesai.GetComponent<Image>().sprite = normalSelesai;
    }

    [Header("Items Button States")]
    [SerializeField] private Sprite normalDaging_1;
    [SerializeField] private Sprite highlightedDaging_1;
    [SerializeField] private Sprite normalAyam_1;
    [SerializeField] private Sprite highlightedAyam_1;
    [SerializeField] private Sprite normalDaging_2;
    [SerializeField] private Sprite highlightedDaging_2;
    [SerializeField] private Sprite normalKambing;
    [SerializeField] private Sprite highlightedKambing;
    [SerializeField] private Sprite normalCumi;
    [SerializeField] private Sprite highlightedCumi;
    [SerializeField] private Sprite normalIkan;
    [SerializeField] private Sprite highlightedIkan;
    [SerializeField] private Sprite normalAyam_2;
    [SerializeField] private Sprite highlightedAyam_2;
    
    public void OnEnterDaging_1() {
        buttonDaging_1.GetComponent<Image>().sprite = highlightedDaging_1;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitDaging_1() {
        buttonDaging_1.GetComponent<Image>().sprite = normalDaging_1;
    }

    public void OnEnterAyam_1() {
        buttonAyam_1.GetComponent<Image>().sprite = highlightedAyam_1;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitAyam_1() {
        buttonAyam_1.GetComponent<Image>().sprite = normalAyam_1;
    }

    public void OnEnterDaging_2() {
        buttonDaging_2.GetComponent<Image>().sprite = highlightedDaging_2;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitDaging_2() {
        buttonDaging_2.GetComponent<Image>().sprite = normalDaging_2;
    }

    public void OnEnterKambing() {
        buttonKambing.GetComponent<Image>().sprite = highlightedKambing;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitKambing() {
        buttonKambing.GetComponent<Image>().sprite = normalKambing;
    }

    public void OnEnterCumi() {
        buttonCumi.GetComponent<Image>().sprite = highlightedCumi;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitCumi() {
        buttonCumi.GetComponent<Image>().sprite = normalCumi;
    }

    public void OnEnterIkan() {
        buttonIkan.GetComponent<Image>().sprite = highlightedIkan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitIkan() {
        buttonIkan.GetComponent<Image>().sprite = normalIkan;
    }

    public void OnEnterAyam_2() {
        buttonAyam_2.GetComponent<Image>().sprite = highlightedAyam_2;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitAyam_2() {
        buttonAyam_2.GetComponent<Image>().sprite = normalAyam_2;
    }
}