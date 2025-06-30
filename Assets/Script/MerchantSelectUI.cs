using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantSelectUI : MonoBehaviour {
    public static MerchantSelectUI Instance { get; private set; }
    [SerializeField] private List<MerchantTypeSO> merchantTypeSOList;
    [SerializeField] private MerchantManager merchantManager;
    
    private List<Transform> merchantButtonList;
    private List<Transform> merchantWindowList;
    private RectTransform rectTransform;
    public GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform merchantBtnTemplate = transform.Find("MerchantBtnTemplate");
        Transform merchantWindowTemplate = transform.Find("MerchantWindowTemplate");
        merchantBtnTemplate.gameObject.SetActive(false);
        merchantWindowTemplate.gameObject.SetActive(false);
        merchantButtonList = new List<Transform>();
        merchantWindowList = new List<Transform>();

        int index = 0;

        foreach (MerchantTypeSO merchantTypeSO in merchantTypeSOList) {
            Transform merchantBtnTransform = Instantiate(merchantBtnTemplate, transform);
            Transform merchantWindowTransform = Instantiate(merchantWindowTemplate, transform);
            merchantBtnTransform.gameObject.SetActive(true);

            merchantBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 115, 0);
            merchantBtnTransform.Find("Image").GetComponent<Image>().sprite = merchantTypeSO.merchantButton;
            merchantWindowTransform.GetComponent<Image>().sprite = merchantTypeSO.merchantWindow;

            merchantBtnTransform.GetComponent<Button>().onClick.AddListener(() => {   
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                
                if (merchantManager.GetActiveMerchantType() == merchantTypeSO) {
                    merchantManager.SetActiveMerchantType(null);
                    DestroyCursorMerchant(); // Hapus kursor jika merchantTypeSO diaktifkan/dinonaktifkan
                    StartCoroutine (merchantManager.ActivateIsMerchantPlaced(0.5f));
                    merchantManager.DestroyPlacementInstance();
                    
                    UIManager.Instance.ActivateUI();
                } else {
                    merchantManager.SetActiveMerchantType(merchantTypeSO);
                    SetCursor(merchantTypeSO.merchantCursor); // Atur kursor saat merchant dipilih
                    StartCoroutine (merchantManager.DeactivateIsMerchantPlaced(0.5f));
                    merchantManager.DestroyPlacementInstance();

                    UIManager.Instance.DeactivateUI();
                    TutorialManager.Instance.isPilihToko = true;
                }
                UpdateSelectedVisual();
            });
            merchantButtonList.Add(merchantBtnTransform);
            merchantWindowList.Add(merchantWindowTransform);

            index++;
        }

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        merchantManager.OnMerchantPlaced += HandleMerchantPlaced;

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            merchantManager.SetActiveMerchantType(null); // Set activeMerchantType ke null
            DestroyCursorMerchant(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });

        Button buttonSpesial = GameObject.Find("ButtonSpesial").GetComponent<Button>(); 
        buttonSpesial.onClick.AddListener(() => {
            merchantManager.SetActiveMerchantType(null); // Set activeMerchantType ke null
            DestroyCursorMerchant(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });
        
        UpdateSelectedVisual();
    }

    private void Update() {
        if (cursorInstance != null) {
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;
            cursorInstance.transform.position = cursorPosition;
        }
        UpdateSelectedVisual();
    }

    private void HandleMerchantPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah merchant ditempatkan
    }

    private void UpdateSelectedVisual() {
        MerchantTypeSO activeMerchantType = merchantManager.GetActiveMerchantType();
        foreach (Transform merchantBtnTransform in merchantButtonList) {
            Image image = merchantBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = merchantBtnTransform.Find("Selected").gameObject;

            if (activeMerchantType != null && image.sprite == activeMerchantType.merchantButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeMerchantType.selectedMerchantButton;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
            }
        }

        foreach (Transform merchantWindowTransform in merchantWindowList) {
            GameObject merchantWindow = merchantWindowTransform.gameObject;

            if (activeMerchantType != null) {
                merchantWindow.SetActive(true);
                merchantWindow.GetComponent<Image>().sprite = activeMerchantType.merchantWindow;
            } else {
                merchantWindow.SetActive(false);
            }
        }
    }

    private void SetCursor(GameObject merchantCursor) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (merchantCursor != null) {
            cursorInstance = Instantiate(merchantCursor);
        }
    }

    public void DestroyCursorMerchant() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }

    [SerializeField] private MerchantTypeSO pemasokRempah;
    public void OnClickTutorialBeliRempah(GameObject button) {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        TutorialManager.Instance.isBeliRempah = true;
        
        merchantManager.SetActiveMerchantType(pemasokRempah);
        SetCursor(pemasokRempah.merchantCursor); // Atur kursor saat merchant dipilih
        StartCoroutine (merchantManager.DeactivateIsMerchantPlaced(0.5f));
        merchantManager.DestroyPlacementInstance();

        UIManager.Instance.DeactivateUI();
        TutorialManager.Instance.isPilihToko = true;
        
        UpdateSelectedVisual();
        button.SetActive(false);
    }
}
