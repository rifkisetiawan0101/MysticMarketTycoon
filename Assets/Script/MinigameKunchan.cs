using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinigameKunchan : MonoBehaviour {
    [SerializeField] private CanvasGroup UI;
    [SerializeField] private GameObject buttonQuest;
    [SerializeField] private GameObject canvasKunchan;
    [SerializeField] private Image kunchanDialog;
    [SerializeField] private Sprite[] kunchanDialogSprites;
    private int dialogIndex;
    [SerializeField] private GameObject blokirMinigame;
    [SerializeField] private GameObject buttonSelanjutnya;
    [SerializeField] private GameObject buttonMulai;
    [SerializeField] private GameObject buttonStop;
    [SerializeField] private GameObject buttonSelesai;

    [SerializeField] private bool isSempurna;
    [SerializeField] private bool isSkillIsu;

    private void Start() {
        kunchanGameGfx.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        buttonQuest.SetActive(false);
        canvasKunchan.SetActive(false);
        countdownMinigame.SetActive(false);
        countdownStart.SetActive(false);
        buttonMulai.SetActive(false);
        potKunchan.enabled = false;
        parameterCook.SetActive(false);
        buttonStop.SetActive(false);

        StartCoroutine(KunchanGIF());
        StartCoroutine(PlaySempurna());
        StartCoroutine(PlaySkillIsu());

        buttonQuest.GetComponent<Button>().onClick.AddListener (() => {
            canvasKunchan.SetActive(true);
            SetUIVisibility(false);
            PersistentManager.Instance.isPlayTimer = false;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonSelanjutnya.GetComponent<Button>().onClick.AddListener (() => {
            dialogIndex++;
            kunchanDialog.GetComponent<Image>().sprite = kunchanDialogSprites[dialogIndex];
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
            StartCoroutine(ParameterMechanics());
            potKunchan.enabled = true;
            StartCoroutine(ParameterGIF());
            buttonStop.SetActive(true);
            buttonMulai.SetActive(false);
            isParameterPlay = true;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonStop.GetComponent<Button>().onClick.AddListener (() => {
            StopCoroutine(ParameterMechanics());
            StopCoroutine(ParameterGIF());
            CookMechanics();
            isParameterPlay = false;
            countdownMinigame.SetActive(false);
            buttonStop.SetActive(false);
            buttonSelesai.SetActive(true);
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
        timer.OnKunchanActive += HandleKunchanActive;
    }

    private void HandleKunchanActive() {
        kunchanGameGfx.SetActive(true);
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
    [SerializeField] private Animator potKunchan;
    [SerializeField] private float parameterTime;
    [SerializeField] private float modTime;
    [SerializeField] private bool isParameterPlay;
    private IEnumerator ParameterMechanics() {
        yield return new WaitUntil(() => isParameterPlay == true);
        yield return new WaitForSeconds(3f);

        while (isParameterPlay) {
            parameterTime += Time.deltaTime;
            modTime = parameterTime % 3f;

            if (modTime == 0f) {
                // Reset parameterTime
                parameterTime = 0f;
            }

            yield return null;
        }
    }

    private void CookMechanics() {
        if (modTime >= 1.4f && modTime <= 1.6f) { // Player berhasil saat mendekati 1.5 detik
            isSempurna = true;
            kunchanDialog.GetComponent<Image>().sprite = kunchanDialogSprites[2];
        } else {
            isSkillIsu = true;
            kunchanDialog.GetComponent<Image>().sprite = kunchanDialogSprites[3];
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

    [SerializeField] private GameObject parameterCook;
    [SerializeField] private Sprite[] parameterFrames;
    private int parameterFrame;
    private IEnumerator ParameterGIF() {
        parameterCook.SetActive(true);
        yield return new WaitForSeconds(3f);
        while (isParameterPlay == true) {
            parameterCook.GetComponent<Image>().sprite = parameterFrames[parameterFrame];
            parameterFrame = (parameterFrame + 1) % parameterFrames.Length;
            yield return new WaitForSeconds(delay_18);
        }
    }

    [Header("Animation")]
    [SerializeField] private float delay_18 = 0.055f; // 18fps
    float audioPlayTime = 0f;
    [SerializeField] private GameObject kunchanGameGfx;
    [SerializeField] private Sprite[] kunchanGameFrames;
    private int kunchanFrame;
    private IEnumerator KunchanGIF() {
        while (true) {
            kunchanGameGfx.GetComponent<SpriteRenderer>().sprite = kunchanGameFrames[kunchanFrame];
            kunchanFrame = (kunchanFrame + 1) % kunchanGameFrames.Length;
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
        canvasKunchan.SetActive(false);
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
}