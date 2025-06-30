using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
    [SerializeField] private Button buttonMenu;
    [SerializeField] private Button closeButtonMenu;
    [SerializeField] private Button simpanButton;
    [SerializeField] private Button mengertiButton;
    [SerializeField] private Button panduanButton;
    [SerializeField] private Button pengaturanButton;
    [SerializeField] private Button closePanduanButton;
    [SerializeField] private Button closePengaturanButton;
    [SerializeField] private Button keluarButton;
    [SerializeField] private Button konfirmasiButton;
    [SerializeField] private Button batalButton;
    [SerializeField] private Button toggleMusikButton;
    [SerializeField] private Button toggleSFXButton;


    [SerializeField] private GameObject windowMenu;
    [SerializeField] private GameObject windowSimpan;
    [SerializeField] private GameObject windowPanduan;
    [SerializeField] private GameObject windowPengaturan;
    [SerializeField] private GameObject windowExit;
    
    [SerializeField] private PenghargaanUI penghargaanUI;

    private bool isMusicAktif = true;
    private bool isSFXAktif = true;

    private void Start() {
        windowMenu.SetActive(false);
        windowPanduan.SetActive(false);
        windowPengaturan.SetActive(false);
        windowExit.SetActive(false);

        buttonMenu.onClick.AddListener(OpenMenuWindow);
        closeButtonMenu.onClick.AddListener(CloseMenuWindow);

        pengaturanButton.onClick.AddListener(() => {
            windowPengaturan.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        simpanButton.onClick.AddListener(() => {
            windowSimpan.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        mengertiButton.onClick.AddListener(() => {
            windowSimpan.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        panduanButton.onClick.AddListener(() => {
            windowPanduan.SetActive(true);
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

        closePanduanButton.onClick.AddListener(() => {
            windowPanduan.SetActive(false);
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

        konfirmasiButton.onClick.AddListener(ExitToHomeScreen);
    }

    private void Update() {
        if (TutorialManager.Instance.isTutorialWaktuPlayed == true) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (windowMenu.activeSelf) { 
                    CloseMenuWindow();
                } else {
                    OpenMenuWindow();
                }
            }
        }
    }

    private void OpenMenuWindow() {
        windowMenu.SetActive(true);
        windowSimpan.SetActive(false);
        windowPanduan.SetActive(false);
        windowPengaturan.SetActive(false);
        windowExit.SetActive(false);
        PersistentManager.Instance.isUIOpen = true;
        penghargaanUI.ClosePenghargaanWindow();
        Time.timeScale = 0;
        Debug.Log("Game Paused. Time.timeScale = " + Time.timeScale);

        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
    }

    private void CloseMenuWindow() {
        windowMenu.SetActive(false);
        windowSimpan.SetActive(false);
        windowPanduan.SetActive(false);
        windowPengaturan.SetActive(false);
        windowExit.SetActive(false);
        PersistentManager.Instance.isUIOpen = false;
        Time.timeScale = 1;
        Debug.Log("Game Un. Time.timeScale = " + Time.timeScale);

        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
    }

    private void ExitToHomeScreen() {
        SceneManager.LoadScene("HomeScreen");
        windowMenu.SetActive(false);
        windowSimpan.SetActive(false);
        windowPanduan.SetActive(false);
        windowPengaturan.SetActive(false);
        windowExit.SetActive(false);
    }

    [Header("Button States")]
    [SerializeField] private Sprite normalMenu;
    [SerializeField] private Sprite highlightedMenu;
    [SerializeField] private Sprite normalSimpan;
    [SerializeField] private Sprite highlightedSimpan;
    [SerializeField] private Sprite normalMengerti;
    [SerializeField] private Sprite highlightedMengerti;
    [SerializeField] private Sprite normalPanduan;
    [SerializeField] private Sprite highlightedPanduan;
    [SerializeField] private Sprite normalPengaturan;
    [SerializeField] private Sprite highlightedPengaturan;
    [SerializeField] private Sprite normalKeluar;
    [SerializeField] private Sprite highlightedKeluar;

    [SerializeField] private Sprite normalBatal;
    [SerializeField] private Sprite highlightedBatal;
    [SerializeField] private Sprite normalKonfirmasi;
    [SerializeField] private Sprite highlightedKonfirmasi;

    [SerializeField] private Sprite toggleAudioAktif;
    [SerializeField] private Sprite toggleAudioMati;

    public void OnEnterMenu() {
        buttonMenu.image.sprite = highlightedMenu;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMenu() {
        buttonMenu.image.sprite = normalMenu;
    }

    public void OnEnterSimpan() {
        simpanButton.image.sprite = highlightedSimpan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSimpan() {
        simpanButton.image.sprite = normalSimpan;
    }

    public void OnEnterPanduan() {
        panduanButton.image.sprite = highlightedPanduan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitPanduan() {
        panduanButton.image.sprite = normalPanduan;
    }

    public void OnEnterPengaturan() {
        pengaturanButton.image.sprite = highlightedPengaturan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitPengaturan() {
        pengaturanButton.image.sprite = normalPengaturan;
    }

    public void OnEnterClosePengaturan()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnEnterKeluar() {
        keluarButton.image.sprite = highlightedKeluar;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitKeluar() {
        keluarButton.image.sprite = normalKeluar;
    }

    public void OnEnterBatal() {
        batalButton.image.sprite = highlightedBatal;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitBatal() {
        batalButton.image.sprite = normalBatal;
    }

    public void OnEnterKonfirmasi() {
        konfirmasiButton.image.sprite = highlightedKonfirmasi;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitKonfirmasi() {
        konfirmasiButton.image.sprite = normalKonfirmasi;
    }

    public void OnEnterMengerti()
    {
        mengertiButton.image.sprite = highlightedMengerti;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMengerti()
    {
        mengertiButton.image.sprite = normalMengerti;
    }
}
