using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {
    public static ShopUI Instance { get; private set; }
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float smoothingSpeed;

    private Vector2 initialPosition;
    private RectTransform rectTransform;
    
    [SerializeField] private GameObject merchantSelectUIObject;
    [SerializeField] private GameObject furniturSelectUIObject;
    [SerializeField] private GameObject spesialSelectUIObject;

    private MerchantSelectUI merchantSelectUI;
    private FurniturSelectUI furniturSelectUI;
    private SpesialSelectUI spesialSelectUI;

    [SerializeField] private Button buttonMerchant;
    [SerializeField] private Button buttonFurnitur;
    [SerializeField] private Button buttonSpesial;
    [SerializeField] private Button buttonCloseShop;
    [SerializeField] private Button buttonOpenShop;

    [SerializeField] private Sprite merchantSelectedSprite;
    [SerializeField] private Sprite merchantNormalSprite;
    [SerializeField] private Sprite furniturSelectedSprite;
    [SerializeField] private Sprite furniturNormalSprite;
    [SerializeField] private Sprite spesialSelectedSprite;
    [SerializeField] private Sprite spesialNormalSprite;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = Vector3.zero; 

        // Ambil komponen MerchantSelectUI dan FurniturSelectUI dari GameObject terkait
        merchantSelectUI = merchantSelectUIObject.GetComponent<MerchantSelectUI>();
        furniturSelectUI = furniturSelectUIObject.GetComponent<FurniturSelectUI>();
        spesialSelectUI = spesialSelectUIObject.GetComponent<SpesialSelectUI>();

        ShowMerchantUI();

        buttonMerchant.onClick.AddListener(() => {
            ShowMerchantUI();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonFurnitur.onClick.AddListener(() => {
            ShowFurniturUI();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonSpesial.onClick.AddListener(() => {
            ShowSpesialUI();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonCloseShop.onClick.AddListener(() => {
            CloseShopUI();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });
        
        buttonOpenShop.onClick.AddListener(() => {
            OpenShopUI();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        UpdateButtonStates();
    }

    private void Update() {
        if (PlayerMovementNew.isMoving) {
            CloseShopUI();
        }
    }

    private Coroutine moveCoroutine;

    public void OpenShopUI() {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        
        FindObjectOfType<PlayerMovementNew>().StopPlayer();
        moveCoroutine = StartCoroutine(MoveShopUI(initialPosition));
    }

    public void CloseShopUI() {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveShopUI(targetPosition));
    }

    private IEnumerator MoveShopUI(Vector2 targetPos) {
        while ((rectTransform.anchoredPosition - targetPos).sqrMagnitude > 0.01f) {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, smoothingSpeed * Time.deltaTime);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;
        UpdateButtonStates();

    }

    public void ShowMerchantUI() {
        merchantSelectUIObject.SetActive(true);
        furniturSelectUIObject.SetActive(false);
        spesialSelectUIObject.SetActive(false);
        
        buttonMerchant.image.sprite = merchantSelectedSprite;
        buttonFurnitur.image.sprite = furniturNormalSprite;
        buttonSpesial.image.sprite = spesialNormalSprite;

        OpenShopUI();
    }

    public void ShowFurniturUI() {
        merchantSelectUIObject.SetActive(false);
        furniturSelectUIObject.SetActive(true);
        spesialSelectUIObject.SetActive(false);
        
        buttonMerchant.image.sprite = merchantNormalSprite;
        buttonFurnitur.image.sprite = furniturSelectedSprite;
        buttonSpesial.image.sprite = spesialNormalSprite;

        OpenShopUI();
    }

    public void ShowSpesialUI() {
        merchantSelectUIObject.SetActive(false);
        furniturSelectUIObject.SetActive(false);
        spesialSelectUIObject.SetActive(true);
        
        buttonMerchant.image.sprite = merchantNormalSprite;
        buttonFurnitur.image.sprite = furniturNormalSprite;
        buttonSpesial.image.sprite = spesialSelectedSprite;

        OpenShopUI();
    }

    private void UpdateButtonStates() {
        bool isAtTargetPosition = (rectTransform.anchoredPosition - targetPosition).sqrMagnitude < 0.01f;
        bool isAtInitialPosition = (rectTransform.anchoredPosition - initialPosition).sqrMagnitude < 0.01f;

        buttonOpenShop.gameObject.SetActive(isAtTargetPosition);
        buttonCloseShop.gameObject.SetActive(isAtInitialPosition);
    }
}
