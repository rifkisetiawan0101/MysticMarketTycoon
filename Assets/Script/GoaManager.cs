using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoaManager : MonoBehaviour {
    public static GoaManager Instance;
    [SerializeField] private GameObject canvasGoa;
    [SerializeField] private GameObject canvasWorldGoa;
    [SerializeField] private Button buttonBukaLahan;
    [SerializeField] private Button buttonKeluar;
    [SerializeField] private Button buttonBeliLahan;
    [SerializeField] private GameObject fadeWhite;
    [SerializeField] private GameObject goaTerbuka;
    [SerializeField] private GameObject jejakKaki;

    private void Start() {
        StartCoroutine(AnimasiButtonBeli());
        goaTerbuka.SetActive(false);
        canvasGoa.SetActive(false);
        canvasWorldGoa.SetActive(false);
        fadeWhite.SetActive(false);

        buttonBukaLahan.onClick.AddListener(() => {
            canvasGoa.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonKeluar.onClick.AddListener(() => {
            canvasGoa.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonBeliLahan.onClick.AddListener(OnClickBeliLahan);
    }

    private void Update() {
        if (PersistentManager.Instance.isBossDefeated == false) {
            buttonBeliLahan.image.sprite = lockedBeliLahan;
            buttonBeliLahan.enabled = false;
            animasiBeli.SetActive(false);
        } else {
            buttonBeliLahan.image.sprite = normalBeliLahan;
            buttonBeliLahan.enabled = true;
            animasiBeli.SetActive(true);
            goaTerbuka.SetActive(true);
            jejakKaki.SetActive(true);
        }
    }

    private void OnClickBeliLahan() {
        if (PersistentManager.Instance.dataKoin >= 200000) {
            StartCoroutine(LoadHomeScreen());
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        }
    }

    private IEnumerator LoadHomeScreen() {
        fadeWhite.SetActive(true);
        yield return new WaitForSeconds (1.5f);
        SceneManager.LoadScene("HomeScreen");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            canvasWorldGoa.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            canvasWorldGoa.SetActive(false);
        }
    }

    private float delay_18 = 0.055f; 
    [SerializeField] private GameObject animasiBeli;
    [SerializeField] private Sprite[] animasiBeliFrames;
    private IEnumerator AnimasiButtonBeli() {
        while (true) {
            for (int i = 0; i < animasiBeliFrames.Length; i++) {
                animasiBeli.GetComponent<Image>().sprite = animasiBeliFrames[i];
                yield return new WaitForSeconds(delay_18);
            }
        }
    }

    [Header("Button States")]
    // di inspektor
    [SerializeField] private Sprite normalBeliLahan;
    [SerializeField] private Sprite highlightedBeliLahan;
    [SerializeField] private Sprite lockedBeliLahan;

    public void OnEnterBeliLahan() {
        buttonBeliLahan.image.sprite = highlightedBeliLahan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitBeliLahan() {
        buttonBeliLahan.image.sprite = normalBeliLahan;
    }

    [SerializeField] private Sprite normalBukaLahan;
    [SerializeField] private Sprite highlightedBukaLahan;

    public void OnEnterBukaLahan() {
        buttonBukaLahan.image.sprite = highlightedBukaLahan;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitBukaLahan() {
        buttonBukaLahan.image.sprite = normalBukaLahan;
    }
}
