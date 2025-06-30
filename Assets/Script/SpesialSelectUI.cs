using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpesialSelectUI : MonoBehaviour {
    [SerializeField] public List<SpesialTypeSO> spesialTypeSOList;
    private List<Transform> spesialButtonList;
    private List<Transform> spesialWindowList;
    private int index = 0;

    [SerializeField] private List<SpesialTerkunciSO> terkunciSOList;
    public List<Transform> terkunciButtonList;
    private List<Transform> terkunciWindowList;
    public int indexTerkunci = 0;

    [SerializeField] private List<SpesialTerpasangSO> terpasangSOList;
    public List<Transform> terpasangButtonList;
    private List<Transform> terpasangWindowList;
    public int indexTerpasang = 0;


    [SerializeField] private SpesialManager spesialManager;
    
    public GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform spesialBtnTemplate = transform.Find("SpesialBtnTemplate");
        Transform spesialWindowTemplate = transform.Find("SpesialWindowTemplate");
        spesialBtnTemplate.gameObject.SetActive(false);
        spesialWindowTemplate.gameObject.SetActive(false);
        spesialButtonList = new List<Transform>();
        spesialWindowList = new List<Transform>();

        foreach (SpesialTypeSO spesialTypeSO in spesialTypeSOList) {
            Transform spesialBtnTransform = Instantiate(spesialBtnTemplate, transform);
            Transform spesialWindowTransform = Instantiate(spesialWindowTemplate, transform);
            spesialBtnTransform.gameObject.SetActive(true);

            spesialBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 115, 0);
            spesialBtnTransform.Find("Image").GetComponent<Image>().sprite = spesialTypeSO.spesialButton;
            spesialWindowTransform.GetComponent<Image>().sprite = spesialTypeSO.spesialWindow;

            spesialBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                
                if (spesialManager.GetActiveSpesialType() == spesialTypeSO) {
                    spesialManager.SetActiveSpesialType(null);
                    DestroyCursorSpesial(); // Hapus kursor jika spesialTypeSO diaktifkan/dinonaktifkan
                    StartCoroutine (spesialManager.ActivateIsSpesialPlaced(0.5f));
                    spesialManager.DestroyPlacementInstance();

                    UIManager.Instance.ActivateUI();
                } else {
                    spesialManager.SetActiveSpesialType(spesialTypeSO);
                    SetCursor(spesialTypeSO.spesialCursor); // Atur kursor saat spesial dipilih
                    StartCoroutine (spesialManager.DeactivateIsSpesialPlaced(0.5f));
                    spesialManager.DestroyPlacementInstance();

                    UIManager.Instance.DeactivateUI();
                }
                UpdateSelectedVisual();
            });
            spesialButtonList.Add(spesialBtnTransform);
            spesialWindowList.Add(spesialWindowTransform);

            index++;
        }

        Transform terkunciBtnTemplate = transform.Find("TerkunciBtnTemplate");
        Transform terkunciWindowTemplate = transform.Find("TerkunciWindowTemplate");
        terkunciBtnTemplate.gameObject.SetActive(false);
        terkunciWindowTemplate.gameObject.SetActive(false);
        terkunciButtonList = new List<Transform>();
        terkunciWindowList = new List<Transform>();

        foreach (SpesialTerkunciSO spesialTerkunciSO in terkunciSOList) {
            int currentIndex = indexTerkunci;
            Transform terkunciBtnTransform = Instantiate(terkunciBtnTemplate, transform);
            Transform terkunciWindowTransform = Instantiate(terkunciWindowTemplate, transform);
            terkunciBtnTransform.gameObject.SetActive(true);

            terkunciBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(indexTerkunci * 115, 0);
            terkunciBtnTransform.Find("Image").GetComponent<Image>().sprite = spesialTerkunciSO.terkunciButton;
            terkunciWindowTransform.GetComponent<Image>().sprite = spesialTerkunciSO.terkunciWindow;

            terkunciBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                if (spesialManager.GetActiveTerkunciType() == spesialTerkunciSO) {
                    spesialManager.SetActiveTerkunciType(null);
                } else {
                    spesialManager.SetActiveTerkunciType(spesialTerkunciSO);
                }
                UpdateSelectedVisual();
            });
            terkunciButtonList.Add(terkunciBtnTransform);
            terkunciWindowList.Add(terkunciWindowTransform);

            indexTerkunci++;
        }

        Transform terpasangBtnTemplate = transform.Find("TerpasangBtnTemplate");
        Transform terpasangWindowTemplate = transform.Find("TerpasangWindowTemplate");
        terpasangBtnTemplate.gameObject.SetActive(false);
        terpasangWindowTemplate.gameObject.SetActive(false);
        terpasangButtonList = new List<Transform>();
        terpasangWindowList = new List<Transform>();

        foreach (SpesialTerpasangSO spesialTerpasangSO in terpasangSOList) {
            int currentIndex = indexTerpasang;
            Transform terpasangBtnTransform = Instantiate(terpasangBtnTemplate, transform);
            Transform terpasangWindowTransform = Instantiate(terpasangWindowTemplate, transform);
            terpasangBtnTransform.gameObject.SetActive(false);

            terpasangBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(indexTerpasang * 115, 0);
            terpasangBtnTransform.Find("Image").GetComponent<Image>().sprite = spesialTerpasangSO.terpasangButton;
            terpasangWindowTransform.GetComponent<Image>().sprite = spesialTerpasangSO.terpasangWindow;

            terpasangBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
                if (spesialManager.GetActiveTerpasangType() == spesialTerpasangSO) {
                    spesialManager.SetActiveTerpasangType(null);
                } else {
                    spesialManager.SetActiveTerpasangType(spesialTerpasangSO);
                }
                UpdateSelectedVisual();
            });
            terpasangButtonList.Add(terpasangBtnTransform);
            terpasangWindowList.Add(terpasangWindowTransform);

            indexTerpasang++;
        }
    }

    private void Start() {
        spesialManager.OnSpesialPlaced += HandleSpesialPlaced;

        Button buttonMerchant = GameObject.Find("ButtonMerchant").GetComponent<Button>(); 
        buttonMerchant.onClick.AddListener(() => {
            spesialManager.SetActiveSpesialType(null);
            spesialManager.SetActiveTerkunciType(null);
            spesialManager.SetActiveTerpasangType(null);
            DestroyCursorSpesial(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            spesialManager.SetActiveSpesialType(null);
            spesialManager.SetActiveTerkunciType(null);
            spesialManager.SetActiveTerpasangType(null);
            DestroyCursorSpesial();
            UpdateSelectedVisual();
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

    private void HandleSpesialPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah spesial ditempatkan
    }

    private void UpdateSelectedVisual() {
        SpesialTypeSO activeSpesialType = spesialManager.GetActiveSpesialType();
        foreach (Transform spesialBtnTransform in spesialButtonList) {
            Image image = spesialBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = spesialBtnTransform.Find("Selected").gameObject;

            if (activeSpesialType != null && image.sprite == activeSpesialType.spesialButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeSpesialType.selectedSpesialButton;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
            }
        }
        foreach (Transform spesialWindowTransform in spesialWindowList) {
            GameObject spesialWindow = spesialWindowTransform.gameObject;

            if (activeSpesialType != null) {
                spesialWindow.SetActive(true);
                spesialWindow.GetComponent<Image>().sprite = activeSpesialType.spesialWindow;
            } else {
                spesialWindow.SetActive(false);
            }
        }

        SpesialTerkunciSO activeTerkunciType = spesialManager.GetActiveTerkunciType();
        foreach (Transform terkunciBtnTransform in terkunciButtonList) {
            Image image = terkunciBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = terkunciBtnTransform.Find("Selected").gameObject;

            if (activeTerkunciType != null && image.sprite == activeTerkunciType.terkunciButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeTerkunciType.selectedTerkunciButton;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
            }
        }
        foreach (Transform terkunciWindowTransform in terkunciWindowList) {
            GameObject terkunciWindow = terkunciWindowTransform.gameObject;

            if (activeTerkunciType != null) {
                terkunciWindow.SetActive(true);
                terkunciWindow.GetComponent<Image>().sprite = activeTerkunciType.terkunciWindow;
            } else {
                terkunciWindow.SetActive(false);
            }
        }

        SpesialTerpasangSO activeTerpasangType = spesialManager.GetActiveTerpasangType();
        foreach (Transform terpasangBtnTransform in terpasangButtonList) {
            Image image = terpasangBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = terpasangBtnTransform.Find("Selected").gameObject;

            if (activeTerpasangType != null && image.sprite == activeTerpasangType.terpasangButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeTerpasangType.selectedTerpasangButton;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
            }
        }
        foreach (Transform terpasangWindowTransform in terpasangWindowList) {
            GameObject terpasangWindow = terpasangWindowTransform.gameObject;

            if (activeTerpasangType != null) {
                terpasangWindow.SetActive(true);
                terpasangWindow.GetComponent<Image>().sprite = activeTerpasangType.terpasangWindow;
            } else {
                terpasangWindow.SetActive(false);
            }
        }
    }
    
    private void SetCursor(GameObject spesialCursor) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (spesialCursor != null) {
            cursorInstance = Instantiate(spesialCursor);
        }
    }

    public void DestroyCursorSpesial() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }
}
