using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurniturSelectUI : MonoBehaviour {
    [SerializeField] private List<FurniturTypeSO> furniturTypeSOList;
    [SerializeField] private FurniturManager furniturManager;
    
    private List<Transform> furniturButtonList;
    private List<Transform> furniturWindowList;
    private RectTransform rectTransform;
    public GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform furniturBtnTemplate = transform.Find("FurniturBtnTemplate");
        Transform furniturWindowTemplate = transform.Find("FurniturWindowTemplate");
        furniturBtnTemplate.gameObject.SetActive(false);
        furniturWindowTemplate.gameObject.SetActive(false);
        furniturButtonList = new List<Transform>();
        furniturWindowList = new List<Transform>();

        int index = 0;

        foreach (FurniturTypeSO furniturTypeSO in furniturTypeSOList) {
            Transform furniturBtnTransform = Instantiate(furniturBtnTemplate, transform);
            Transform furniturWindowTransform = Instantiate(furniturWindowTemplate, transform);
            furniturBtnTransform.gameObject.SetActive(true);

            furniturBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 115, 0);
            furniturBtnTransform.Find("Image").GetComponent<Image>().sprite = furniturTypeSO.furniturButton;
            furniturWindowTransform.GetComponent<Image>().sprite = furniturTypeSO.furniturWindow;

            furniturBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);

                if (furniturManager.GetActiveFurniturType() == furniturTypeSO) {
                    furniturManager.SetActiveFurniturType(null);
                    DestroyCursorFurnitur(); // Hapus kursor jika furniturTypeSO diaktifkan/dinonaktifkan
                    StartCoroutine (furniturManager.ActivateIsFurniturPlaced(0.5f));
                    furniturManager.DestroyPlacementInstance();

                    UIManager.Instance.ActivateUI();
                } else {
                    furniturManager.SetActiveFurniturType(furniturTypeSO);
                    SetCursor(furniturTypeSO.furniturCursor); // Atur kursor saat furnitur dipilih
                    StartCoroutine (furniturManager.DeactivateIsFurniturPlaced(0.5f));
                    furniturManager.DestroyPlacementInstance();

                    UIManager.Instance.DeactivateUI();
                }
                UpdateSelectedVisual();
            });
            furniturButtonList.Add(furniturBtnTransform);
            furniturWindowList.Add(furniturWindowTransform);

            index++;
        }

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        furniturManager.OnFurniturPlaced += HandleFurniturPlaced;

        Button buttonMerchant = GameObject.Find("ButtonMerchant").GetComponent<Button>(); 
        buttonMerchant.onClick.AddListener(() => {
            furniturManager.SetActiveFurniturType(null); // Set activeFurniturType ke null
            DestroyCursorFurnitur(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });
        
        Button buttonSpesial = GameObject.Find("ButtonSpesial").GetComponent<Button>(); 
        buttonSpesial.onClick.AddListener(() => {
            furniturManager.SetActiveFurniturType(null); // Set activeFurniturType ke null
            DestroyCursorFurnitur(); // Hapus kursor
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

    private void HandleFurniturPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah furnitur ditempatkan
    }

    private void UpdateSelectedVisual() {
        FurniturTypeSO activeFurniturType = furniturManager.GetActiveFurniturType();
        foreach (Transform furniturBtnTransform in furniturButtonList) {
            Image image = furniturBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = furniturBtnTransform.Find("Selected").gameObject;

            if (activeFurniturType != null && image.sprite == activeFurniturType.furniturButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeFurniturType.selectedFurniturButton;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
            }
        }

        foreach (Transform furniturWindowTransform in furniturWindowList) {
            GameObject furniturWindow = furniturWindowTransform.gameObject;

            if (activeFurniturType != null) {
                furniturWindow.SetActive(true);
                furniturWindow.GetComponent<Image>().sprite = activeFurniturType.furniturWindow;
            } else {
                furniturWindow.SetActive(false);
            }
        }
    }

    private void SetCursor(GameObject furniturCursor) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (furniturCursor != null) {
            cursorInstance = Instantiate(furniturCursor);
        }
    }

    public void DestroyCursorFurnitur() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }
}
