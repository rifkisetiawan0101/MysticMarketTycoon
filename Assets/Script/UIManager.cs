using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance;
    [SerializeField] private GameObject BlackFadeOut;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject invoiceUI;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject tasUI;
    [SerializeField] private GameObject blokirUI;
    [SerializeField] private Timer timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        BlackFadeOut.SetActive(true);
    }

    private void Start()
    {   
        // Set visibilitas image(2) sesuai dengan scene saat ini
        UpdateImageVisibility();

        StartCoroutine(DisableOutroOverlay(1.1f));
    }

    private IEnumerator DisableOutroOverlay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        BlackFadeOut.SetActive(false);
    }


    ///---------------------


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update visibilitas image(2) ketika scene baru dimuat
        UpdateImageVisibility();
        UpdateUI();
    }

    private void UpdateImageVisibility() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "InGame") {
            shopUI.SetActive(false);
            invoiceUI.SetActive(false);
            timerUI.SetActive(false);
            timer.elapsedTime = 0;
            PersistentManager.Instance.isInvoiceShown = false;
            ActivateUI();
        }
        else {
            shopUI.SetActive(true);
            timerUI.SetActive(true);
        }
    }

    private void UpdateUI() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "InGame" || sceneName == "InGamePagi") {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    public void DeactivateUI() {
        blokirUI.SetActive(true);
        PersistentManager.Instance.isActivateUI = false;
    }

    public void ActivateUI() {
        blokirUI.SetActive(false);
        PersistentManager.Instance.isActivateUI = true;
    }
}
