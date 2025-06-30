using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class MerchantManager : MonoBehaviour {
    public static MerchantManager Instance { get; private set; }

    [SerializeField] private MerchantTypeSO activeMerchantType;
    [SerializeField] private MerchantTerkunciSO activeTerkunci;
    [SerializeField] private LayerMask ignoreLayerMask;

    private MerchantSelectUI merchantSelectUI;
    private bool isPlacingMerchant = false;
    public bool isMerchantPlaced = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    public event Action OnMerchantPlaced;
    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingMerchant) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnMerchant(activeMerchantType, mouseWorldPosition)) {
                PlacementInstance();
                UIManager.Instance.DeactivateUI();
            } else if (LessKoin(activeMerchantType)) {
                StartCoroutine(FeedbackManager.instance.PlayLessKoinNotif());
                DestroyPlacementInstance();
                merchantSelectUI.DestroyCursorMerchant();
                isMerchantPlaced = true;
                SetActiveMerchantType(null);
                UIManager.Instance.ActivateUI();
            } else if (LahanBuruk(activeMerchantType, mouseWorldPosition)) {
                StartCoroutine(FeedbackManager.instance.PlayLahanBurukNotif());
                DestroyPlacementInstance();
                merchantSelectUI.DestroyCursorMerchant();
                isMerchantPlaced = true;
                SetActiveMerchantType(null);
                UIManager.Instance.ActivateUI();
            }
        }

        merchantSelectUI = FindObjectOfType<MerchantSelectUI>();
    }

    private Transform placementInstance;
    private void PlacementInstance() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.placementSound);
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

        Vector3 spawnPosition = mouseWorldPosition;
        placementInstance = Instantiate(activeMerchantType.merchantPlacementPrefab, spawnPosition, Quaternion.identity);

        // Set status isPlacingMerchant menjadi true setelah memanggil placement
        isPlacingMerchant = true;

        // Setup merchant placement instance
        MerchantPlacement merchantPlacement = placementInstance.GetComponent<MerchantPlacement>();
        merchantPlacement.Setup(spawnPosition, this);
        merchantSelectUI.DestroyCursorMerchant();
        activeMerchantTypeName = activeMerchantType.merchantName;

        StartCoroutine (ActivateIsMerchantPlaced (0.5f));
    }

    public void DestroyPlacementInstance() {
        if (placementInstance != null) {
            Destroy(placementInstance.gameObject);
            placementInstance = null;
            isPlacingMerchant = false;
        }
    }

    [SerializeField] GameObject kunchanSpawner;
    [SerializeField] GameObject pocinSpawner;
    [SerializeField] GameObject ayangSpawner;
    [SerializeField] GameObject utoSpawner;
    [SerializeField] GameObject uwoSpawner;

    private int merchantIndex = -1;
    public int rotateIndex = 0;

    public int GetCurrentMerchantIndex() {
        return merchantIndex;
    }

    public int GetCurrentRotateIndex() {
        return rotateIndex;
    }

    private string activeMerchantTypeName;
    public string GetActiveMerchantTypeName() {
        return activeMerchantTypeName;
    }

    public void MerchantPlacing(Vector3 position) {  
        Instantiate(activeMerchantType.merchantConstructionPrefab, position, Quaternion.identity);

        PersistentManager.Instance.UpdateKoin(-activeMerchantType.merchantPrice);

        merchantIndex++;
        PersistentManager.MerchantData newMerchantData = new PersistentManager.MerchantData {
            merchantTypeSO = activeMerchantType, // Atur merchantTypeSO
            merchantPosition = position
        };
        PersistentManager.Instance.dataMerchantList.Add(newMerchantData);

        HandleJuraganPasarEvent();
        
        if (SceneManager.GetActiveScene().name == "InGame") {
            pocinSpawner.SetActive(true);
            kunchanSpawner.SetActive(true);
            ayangSpawner.SetActive(true);
            utoSpawner.SetActive(true);
            uwoSpawner.SetActive(true);
        } else {
            pocinSpawner.SetActive(false);
            kunchanSpawner.SetActive(false);
            ayangSpawner.SetActive(false);
            utoSpawner.SetActive(false);
            uwoSpawner.SetActive(false);
        }

        SetActiveMerchantType(null);
        OnMerchantPlaced?.Invoke(); // Panggil event ketika merchant ditempatkan
        isPlacingMerchant = false; // Reset status placement
        PersistentManager.Instance.UpdateTotalMerchant(1f);
        PersistentManager.Instance.UpdateTotalNpc(3f);

        UIManager.Instance.ActivateUI();

        TutorialManager.Instance.isTaruhMerchant = true;

        StartCoroutine(ScanAstarAfterWait(2f));

        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.placeAndUpgradeBuilding);
    }

    public static event Action OnJuraganPasar;
    public void HandleJuraganPasarEvent() {
        if (activeMerchantType.merchantName == "Pedagang Sayur") {
            PersistentManager.Instance.isSayurPlaced = true;
        }

        if (activeMerchantType.merchantName == "Pemasok Rempah") {
            PersistentManager.Instance.isRempahPlaced = true;
        }

        if (activeMerchantType.merchantName == "Penjual Daging") {
            PersistentManager.Instance.isDagingPlaced = true;
        }
        OnJuraganPasar?.Invoke();
    }

    public void CancelPlacement() {
        rotateIndex = 0;
        SetActiveMerchantType(null);
        isPlacingMerchant = false; // Reset status placement jika dibatalkan
        UIManager.Instance.ActivateUI();
    }

    public void SetActiveMerchantType(MerchantTypeSO merchantTypeSO) {
        activeMerchantType = merchantTypeSO;
        DestroyPlacementInstance();
        merchantSelectUI.DestroyCursorMerchant();
    }

    public MerchantTypeSO GetActiveMerchantType() {
        return activeMerchantType;
    }

    public void SetActiveTerkunci(MerchantTerkunciSO merchantTerkunciSO) {
        activeTerkunci = merchantTerkunciSO;
    }

    public MerchantTerkunciSO GetActiveTerkunci() {
        return activeTerkunci;
    }

    private bool CanSpawnMerchant(MerchantTypeSO merchantTypeSO, Vector3 position) {
        if (activeMerchantType == null) {
            return false;
        }

        if (PersistentManager.Instance.dataKoin < merchantTypeSO.merchantPrice) {
            return false;
        }

        PolygonCollider2D merchantCollider = merchantTypeSO.merchantPrefab.GetComponent<PolygonCollider2D>();

        if (merchantCollider == null) {
            Debug.LogError("PolygonCollider2D tidak ditemukan pada prefab merchant!");
            return false;
        }

        Vector2[] worldSpacePoints = new Vector2[merchantCollider.points.Length];

        for (int i = 0; i < merchantCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + merchantCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return false;
            }
        }

        return true;
    }

    private bool LessKoin(MerchantTypeSO merchantTypeSO) {
        if (PersistentManager.Instance.dataKoin < merchantTypeSO.merchantPrice) {
            return true;
        }
        return false;
    }

    private bool LahanBuruk(MerchantTypeSO merchantTypeSO, Vector3 position) {
        PolygonCollider2D merchantCollider = merchantTypeSO.merchantPrefab.GetComponent<PolygonCollider2D>();

        Vector2[] worldSpacePoints = new Vector2[merchantCollider.points.Length];

        for (int i = 0; i < merchantCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + merchantCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return true;
            }
        }
        return false;
    }

    public IEnumerator ActivateIsMerchantPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isMerchantPlaced = true;
    }

    public IEnumerator DeactivateIsMerchantPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isMerchantPlaced = false;
    }

    private IEnumerator ScanAstarAfterWait(float waitTime) {
        GameObject aStarObject = GameObject.Find("A_Star");
        yield return new WaitForSeconds(waitTime);

        if (aStarObject != null) {
            AstarPath pathfinder = aStarObject.GetComponent<AstarPath>(); // Menggunakan AstarPath
            if (pathfinder != null) {
                pathfinder.Scan(); // Panggil metode Scan
            } else {
                Debug.LogWarning("Komponen AstarPath tidak ditemukan pada GameObject A_Star!");
            }
        } else { 
            Debug.LogWarning("GameObject A_Star tidak ditemukan di scene!");
        }
    }
}