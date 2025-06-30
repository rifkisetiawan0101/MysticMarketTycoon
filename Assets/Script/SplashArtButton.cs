using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashArtButton : MonoBehaviour
{
    public string sceneName = "FirstCutScene";
    private float delayTime = 5.3f;


    [SerializeField] private GameObject blackFadeIn;

    // ----------------------------------------------------------------------------NEW---------------------------------------------------------

    [SerializeField] private Button mulaiBaruButton;
    [SerializeField] private Button lanjutkanButton;
    [SerializeField] private Button kreditButton;
    [SerializeField] private Button keluarButton;
    [SerializeField] private Button pengaturanButton;

    [SerializeField] private GameObject lanjutkanWindow;
    [SerializeField] private Button mengertiButton;

    [SerializeField] private Button konfirmasiButton;
    [SerializeField] private Button batalButton;
    [SerializeField] private Button closePengaturanButton;
    [SerializeField] private Button toggleMusikButton;
    [SerializeField] private Button toggleSFXButton;

    [SerializeField] private GameObject windowPengaturan;
    [SerializeField] private GameObject WindowKredit;
    [SerializeField] private GameObject windowExit;

    [SerializeField] private GameObject blockOverlay;

    private bool isMusicAktif = true;
    private bool isSFXAktif = true;

    [Header("Button Sprite States")]
    [SerializeField] private Sprite normalPengaturan;
    [SerializeField] private Sprite highlightedPengaturan;

    [SerializeField] private Sprite normalMulaiBaru;
    [SerializeField] private Sprite highlightMulaiBaru;

    [SerializeField] private Sprite normalLanjutkan;
    [SerializeField] private Sprite highlightLanjutkan;

    [SerializeField] private Sprite normalMengerti;
    [SerializeField] private Sprite highlightMengerti;

    [SerializeField] private Sprite normalKredit;
    [SerializeField] private Sprite highlightKredit;
    [SerializeField] private Sprite normalSkip;
    [SerializeField] private Sprite highlightSkip;

    [SerializeField] private Sprite normalKeluar;
    [SerializeField] private Sprite highlightKeluar;

    [SerializeField] private Sprite normalBatal;
    [SerializeField] private Sprite highlightedBatal;

    [SerializeField] private Sprite normalKonfirmasi;
    [SerializeField] private Sprite highlightedKonfirmasi;

    [SerializeField] private Sprite toggleAudioAktif;
    [SerializeField] private Sprite toggleAudioMati;

    // -----------------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        //New

        StartCoroutine(UnblockClick());

        //mulaibaru
        mulaiBaruButton.onClick.AddListener(() => {
            ActivateIntroOverlay();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            AudioManager.audioManager.FadeOutMusic(0.5f); // Durasi fade out 0.5 detik
            StartCoroutine(LoadSceneAfterDelay(0.5f));
        });

        //kredit
        kreditButton.onClick.AddListener(() => {
            WindowKredit.SetActive(true);
            StartCoroutine(PlayCreditRoll());
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        skipKredit.GetComponent<Button>().onClick.AddListener(() => {
            WindowKredit.SetActive(false);
            kreditSkipped = true;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        pengaturanButton.onClick.AddListener(() => {
            windowPengaturan.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        toggleMusikButton.onClick.AddListener(() =>
        {
            if (isMusicAktif == true)
            {
                toggleMusikButton.image.sprite = toggleAudioMati;
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                isMusicAktif = false;
                AudioManager.audioManager.musicSource.Pause();
            }
            else
            {
                toggleMusikButton.image.sprite = toggleAudioAktif;
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                isMusicAktif = true;
                AudioManager.audioManager.musicSource.Play();
            }
        });

        toggleSFXButton.onClick.AddListener(() =>
        {
            if (isSFXAktif == true)
            {
                toggleSFXButton.image.sprite = toggleAudioMati;
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                isSFXAktif = false;
                AudioManager.audioManager.SFXSource.mute = true;
            }
            else
            {
                toggleSFXButton.image.sprite = toggleAudioAktif;
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                isSFXAktif = true;
                AudioManager.audioManager.SFXSource.mute = false;
            }
        });

        closePengaturanButton.onClick.AddListener(() => {
            windowPengaturan.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        lanjutkanButton.onClick.AddListener(() => {
            lanjutkanWindow.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        mengertiButton.onClick.AddListener(() => {
            lanjutkanWindow.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        keluarButton.onClick.AddListener(() => {
            windowExit.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        batalButton.onClick.AddListener(() => {
            windowExit.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        konfirmasiButton.onClick.AddListener(ExitGame);

        StartCoroutine(PlayGifLogo());

        // -----------------------------------------------------------------------------------------
    }

    //private IEnumerator EnableButtonAfterDelay()
    //{
    //    // Tunggu selama 5.3 detik
    //    yield return new WaitForSeconds(delayTime);

    //    // Button btn = GetComponent<Button>(); pake ini kalo kodenya berada didalem objek button itu sendiri
    //    mulaiBaruButton = GameObject.Find("MulaiBaruButton").GetComponent<Button>();
    //    lanjutkanButton = GameObject.Find("LanjutkanButton").GetComponent<Button>();
    //    kreditButton = GameObject.Find("KreditButton").GetComponent<Button>();
    //    keluarButton = GameObject.Find("KeluarButton").GetComponent<Button>();
    //    pengaturanButton = GameObject.Find("PengaturanButton").GetComponent<Button>();
    //}

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        LoadScene();
    }

    private void ActivateIntroOverlay()
    {
        blackFadeIn.SetActive(true);
    }

    private void ExitGame()
    {
        // Jika sedang dalam editor, gunakan ini
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Jika dalam build, gunakan ini
        Application.Quit();
        #endif 
    }

    [SerializeField] private GameObject kreditRoll;
    [SerializeField] private GameObject skipKredit;
    [SerializeField] private bool kreditSkipped = false;
    private IEnumerator PlayCreditRoll() {
        kreditRoll.SetActive(true);
        skipKredit.SetActive(false);
        yield return new WaitForSeconds(10f);
        skipKredit.SetActive(true);

        yield return new WaitUntil(() => kreditSkipped == true);
        kreditRoll.SetActive(false);
        skipKredit.SetActive(false);
        kreditSkipped = false;
    }

    [SerializeField] private Image mysticImage;
    [SerializeField] private Sprite[] mysticFrames;
    private float frameDelay = 0.041f;
    private int currentFrame;
    private IEnumerator PlayGifLogo() {
        while (true) {
            mysticImage.sprite = mysticFrames[currentFrame];
            currentFrame = (currentFrame + 1) % mysticFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }


    ///// NEW --------------------------------------------------------------------------------------------------------------------------
    ///

    public void OnEnterMulaiBaru()
    {
        mulaiBaruButton.image.sprite = highlightMulaiBaru;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMulaiBaru()
    {
        mulaiBaruButton.image.sprite = normalMulaiBaru;
    }

    //

    public void OnEnterLanjutkan()
    {
        lanjutkanButton.image.sprite = highlightLanjutkan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitLanjutkan()
    {
        lanjutkanButton.image.sprite = normalLanjutkan;
    }


    public void OnEnterMengerti()
    {
        mengertiButton.image.sprite = highlightMengerti;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMengerti()
    {
        mengertiButton.image.sprite = normalMengerti;
    }


    //

    public void OnEnterKredit()
    {
        kreditButton.image.sprite = highlightKredit;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitKredit()
    {
        kreditButton.image.sprite = normalKredit;
    }


    public void OnEnterSkip()
    {
        skipKredit.GetComponent<Button>().image.sprite = highlightSkip;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSkip()
    {
        skipKredit.GetComponent<Button>().image.sprite = normalSkip;
    }


    //

    public void OnEnterKeluar()
    {
        keluarButton.image.sprite = highlightKeluar;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitKeluar()
    {
        keluarButton.image.sprite = normalKeluar;
    }



    //

    public void OnEnterPengaturan()
    {
        pengaturanButton.image.sprite = highlightedPengaturan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitPengaturan()
    {
        pengaturanButton.image.sprite = normalPengaturan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnEnterClosePengaturan()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnEnterBatal()
    {
        batalButton.image.sprite = highlightedBatal;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitBatal()
    {
        batalButton.image.sprite = normalBatal;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnEnterKonfirmasi()
    {
        konfirmasiButton.image.sprite = highlightedKonfirmasi;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitKonfirmasi()
    {
        konfirmasiButton.image.sprite = normalKonfirmasi;
    }

    private IEnumerator UnblockClick()
    {
        yield return new WaitForSeconds(delayTime);

        blockOverlay.SetActive(false);
    }
}
