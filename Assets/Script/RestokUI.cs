using UnityEngine;
using UnityEngine.UI;

public class RestokUI : MonoBehaviour {
    [SerializeField] private Button buttonOpenRestok;
    [SerializeField] private Button buttonBeliSayur;
    [SerializeField] private Button buttonBeliRempah;
    [SerializeField] private Button buttonBeliDaging;
    [SerializeField] private Button buttonCloseRestok;
    [SerializeField] private GameObject windowRestok;
    [SerializeField] private GameObject overlay;

    private PremanButoAI uto;
    private NpcAI npc;

    private void Start() {
        uto = FindObjectOfType<PremanButoAI>();
        npc = FindObjectOfType<NpcAI>();

        Destroy(uto);
        Destroy(npc);

        overlay.SetActive(false);
        
        buttonOpenRestok.onClick.AddListener(() => {
            TutorialManager.Instance.isRestokOpen = true;
            windowRestok.SetActive(true);
            overlay.SetActive(true);
            FindObjectOfType<PlayerMovementNewPagi>().StopPlayer();

            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonBeliSayur.GetComponent<Button>().onClick.AddListener(() => {
            if (PersistentManager.Instance.dataKoin >= 10) {
                PersistentManager.Instance.dataKoin -= 10;
                PersistentManager.Instance.dataStokSayur++;
                StartCoroutine(FeedbackManager.instance.PlayBeliSayur());
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buyItem);
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            }
        });

        buttonBeliRempah.GetComponent<Button>().onClick.AddListener(() => {
            if (PersistentManager.Instance.dataKoin >= 25) {
                PersistentManager.Instance.dataKoin -= 25;
                PersistentManager.Instance.dataStokRempah++;
                StartCoroutine(FeedbackManager.instance.PlayBeliRempah());
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buyItem);
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            }
        });

        buttonBeliDaging.GetComponent<Button>().onClick.AddListener(() => {
            if (PersistentManager.Instance.dataKoin >= 50) {
                PersistentManager.Instance.dataKoin -= 50;
                PersistentManager.Instance.dataStokDaging++;
                StartCoroutine(FeedbackManager.instance.PlayBeliDaging());
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buyItem);
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
            }
        });

        buttonCloseRestok.GetComponent<Button>().onClick.AddListener(() => {
            windowRestok.SetActive(false);
            overlay.SetActive(false);
            TutorialManager.Instance.isRestokOpen = false;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });
    }

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnHighlightSayur() {
        buttonBeliSayur.image.sprite = highlightedSprite;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnHighlightSayur() {
        buttonBeliSayur.image.sprite = normalSprite;
    }

    public void OnHighlightRempah() {
        buttonBeliRempah.image.sprite = highlightedSprite;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnHighlightRempah() {
        buttonBeliRempah.image.sprite = normalSprite;
    }

    public void OnHighlightDaging() {
        buttonBeliDaging.image.sprite = highlightedSprite;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnHighlightDaging() {
        buttonBeliDaging.image.sprite = normalSprite;
    }

    [SerializeField] private Sprite normalRestok;
    [SerializeField] private Sprite highlightedRestok;

    public void OnHighlightRestok() {
        buttonOpenRestok.image.sprite = highlightedRestok;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnUnHighlightRestok() {
        buttonOpenRestok.image.sprite = normalRestok;
    }
}
