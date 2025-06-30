using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BukaPasarUI : MonoBehaviour {
    public static BukaPasarUI Instance;
    [SerializeField] private GameObject bukaPasarWindow;
    [SerializeField] private Button buttonOpenWindow;
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonMenujuMalam;
    [SerializeField] private GameObject overlay;

    private void Awake() {
        UIManager uIManager = FindObjectOfType<UIManager>();
        uIManager.gameObject.SetActive(true);
    }
    
    private void Start() {
        overlay.SetActive(false);
        bukaPasarWindow.SetActive(false);

        buttonOpenWindow.onClick.AddListener(OnOpenWindow);

        buttonMenujuMalam.onClick.AddListener(OnMenujuMalam);

        buttonClose.onClick.AddListener(OnCloseWindow);
    }

    public void OnMenujuMalam() {
        overlay.SetActive(false);
        SceneManager.LoadScene("InGame");
        PersistentManager.Instance.UpdateNightCounter(1);
        PersistentManager.Instance.isNowMalam = true;
        PersistentManager.Instance.dataTotalSpawnNpc = 0;
        Debug.Log ("Night COunter = " + PersistentManager.Instance.nightCounter);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.sceneTransitionSound);
            
        PremanSpawner premanSpawner = FindObjectOfType<PremanSpawner>();
        premanSpawner.gameObject.SetActive(true);
    }

    public void OnOpenWindow() {
        bukaPasarWindow.SetActive(true);
        overlay.SetActive(true);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
    }
    
    public void OnCloseWindow() {
        bukaPasarWindow.SetActive(false);
        overlay.SetActive(false);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
    }

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnHighlightButton() {
        buttonOpenWindow.image.sprite = highlightedSprite;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnhighlightButton() {
        buttonOpenWindow.image.sprite = normalSprite;
    }

    [SerializeField] private Image buttonMenujuMalamImage;
    [SerializeField] private Sprite normalMalam;
    [SerializeField] private Sprite highlightedMalam;

    public void OnEnterMalam() {
        buttonMenujuMalamImage.sprite = highlightedMalam;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMalam() {
        buttonMenujuMalamImage.sprite = normalMalam;
    }
}