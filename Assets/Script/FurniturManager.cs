using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class FurniturManager : MonoBehaviour {
    public static FurniturManager Instance { get; private set; }

    [SerializeField] private FurniturTypeSO activeFurniturType;
    [SerializeField] private LayerMask ignoreLayerMask;

    private FurniturSelectUI furniturSelectUI;
    private bool isPlacingFurnitur = false;
    public bool isFurniturPlaced = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    public event Action OnFurniturPlaced;
    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingFurnitur) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnFurnitur(activeFurniturType, mouseWorldPosition)) {
                PlacementInstance();
                UIManager.Instance.DeactivateUI();
            } else if (LessKoin(activeFurniturType)) {
                StartCoroutine(FeedbackManager.instance.PlayLessKoinNotif());
                DestroyPlacementInstance();
                furniturSelectUI.DestroyCursorFurnitur();
                isFurniturPlaced = true;
                SetActiveFurniturType(null);
                UIManager.Instance.ActivateUI();
            } else if (LahanBuruk(activeFurniturType, mouseWorldPosition)) {
                StartCoroutine(FeedbackManager.instance.PlayLahanBurukNotif());
                DestroyPlacementInstance();
                furniturSelectUI.DestroyCursorFurnitur();
                isFurniturPlaced = true;
                SetActiveFurniturType(null);
                UIManager.Instance.ActivateUI();
            }
        }
        furniturSelectUI = FindObjectOfType<FurniturSelectUI>();
    }

    private Transform placementInstance;
    private void PlacementInstance() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.placementSound);
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

        Vector3 spawnPosition = mouseWorldPosition;
        placementInstance = Instantiate(activeFurniturType.furniturPlacementPrefab, spawnPosition, Quaternion.identity);

        // Set status isPlacingFurnitur menjadi true setelah memanggil placement
        isPlacingFurnitur = true;

        // Setup furnitur placement instance
        FurniturPlacement furniturPlacement = placementInstance.GetComponent<FurniturPlacement>();
        furniturPlacement.Setup(spawnPosition, this);
        furniturSelectUI.DestroyCursorFurnitur();

        StartCoroutine (ActivateIsFurniturPlaced (0.5f));
    }

    public void DestroyPlacementInstance() {
        if (placementInstance != null) {
            Destroy(placementInstance.gameObject);
            placementInstance = null;
            isPlacingFurnitur = false;
        }
    }

    public void FurniturPlacing(Vector3 position) {
        Instantiate(activeFurniturType.furniturConstructionPrefab, position, Quaternion.identity);

        PersistentManager.Instance.UpdateKoin(-activeFurniturType.furniturPrice);

        PersistentManager.FurniturData newFurniturData = new PersistentManager.FurniturData {
            furniturTypeSO = activeFurniturType, // Atur FurniturTypeSO
            furniturPosition = position
        };

        PersistentManager.Instance.dataFurniturList.Add(newFurniturData);

        SetActiveFurniturType(null); // Reset activeFurniturType setelah menaruh furnitur
        OnFurniturPlaced?.Invoke(); // Panggil event ketika furnitur ditempatkan
        isPlacingFurnitur = false; // Reset status placement

        UIManager.Instance.ActivateUI();

        StartCoroutine(ScanAstarAfterWait(2f));

        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.placeAndUpgradeBuilding);
    }

    public void CancelPlacement() {
        SetActiveFurniturType(null);
        isPlacingFurnitur = false; // Reset status placement jika dibatalkan
        UIManager.Instance.ActivateUI();
    }

    public void SetActiveFurniturType(FurniturTypeSO furniturTypeSO) {
        activeFurniturType = furniturTypeSO;
        DestroyPlacementInstance();
        furniturSelectUI.DestroyCursorFurnitur();
    }

    public FurniturTypeSO GetActiveFurniturType() {
        return activeFurniturType;
    }

    private bool CanSpawnFurnitur(FurniturTypeSO furniturTypeSO, Vector3 position) {
        if (activeFurniturType == null) {
            return false;
        }

        if (PersistentManager.Instance.dataKoin <= furniturTypeSO.furniturPrice) {
            return false;
        }

        PolygonCollider2D furniturCollider = furniturTypeSO.furniturPrefab.GetComponent<PolygonCollider2D>();

        if (furniturCollider == null) {
            Debug.LogError("PolygonCollider2D tidak ditemukan pada prefab furnitur!");
            return false;
        }

        Vector2[] worldSpacePoints = new Vector2[furniturCollider.points.Length];

        for (int i = 0; i < furniturCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + furniturCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return false;
            }
        }

        return true;
    }

    private bool LessKoin(FurniturTypeSO furniturTypeSO) {
        if (PersistentManager.Instance.dataKoin < furniturTypeSO.furniturPrice) {
            return true;
        }
        return false;
    }

    private bool LahanBuruk(FurniturTypeSO furniturTypeSO, Vector3 position) {
        PolygonCollider2D furniturCollider = furniturTypeSO.furniturPrefab.GetComponent<PolygonCollider2D>();

        Vector2[] worldSpacePoints = new Vector2[furniturCollider.points.Length];

        for (int i = 0; i < furniturCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + furniturCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return true;
            }
        }
        return false;
    }

    public IEnumerator ActivateIsFurniturPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isFurniturPlaced = true;
    }

    public IEnumerator DeactivateIsFurniturPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isFurniturPlaced = false;
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
