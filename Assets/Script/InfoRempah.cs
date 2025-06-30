using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class InfoRempah : MonoBehaviour, IMerchant {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    
    [Header("---Window---")]
    [SerializeField] private GameObject infoRempahCanvas;
    [SerializeField] private TextMeshProUGUI teksLevelRempah;
    [SerializeField] private TextMeshProUGUI teksHargaRempah;
    [SerializeField] private TextMeshProUGUI teksStokDagangan; // dengan max stok
    [SerializeField] private TextMeshProUGUI teksUpLevel;
    [SerializeField] private TextMeshProUGUI teksHargaUpgradeRempah;
    [SerializeField] private TextMeshProUGUI teksRempahBatu;
    [SerializeField] private TextMeshProUGUI teksRempahKayu;
    [SerializeField] private TextMeshProUGUI teksRempahTanahLiat;
    [SerializeField] private TextMeshProUGUI teksBatuAkik;
    [SerializeField] private Button closeInfoButton;
    [SerializeField] private Button restokDaganganButton;

    [Header("---Panel---")]
    [SerializeField] private GameObject tingkatkanButton;
    [SerializeField] private GameObject maksUpgrade; 
    [SerializeField] private GameObject konfirmasiPanel;
    [SerializeField] private Button konfirmasiButton;
    [SerializeField] private TextMeshProUGUI teksKonfirm;
    [SerializeField] private TextMeshProUGUI upLevel;
    [SerializeField] private TextMeshProUGUI upMaxStok;
    [SerializeField] private TextMeshProUGUI upHarga;
    [SerializeField] private TextMeshProUGUI maxStok;
    [SerializeField] private TextMeshProUGUI harga;
    [SerializeField] private Button batalButton;

    [Header("---Info Luar---")]
    [SerializeField] private TextMeshProUGUI teksPenghasilanRempah;
    [SerializeField] private TextMeshProUGUI teksStokLuar;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        string currentSceneName = scene.name;

        if (currentSceneName == "InGame" || currentSceneName == "HomeScreen") {
            gameObject.SetActive(true);
            ScanAstar();
        } else {
            foreach (var merchantData in PersistentManager.Instance.dataMerchantList) {
                merchantData.merchantObject.SetActive(false);
            }
        }

        if (currentSceneName == "HomeScreen") {
            Destroy(gameObject);
        }
    }

    public int merchantIndex;
    private int rotateIndex;

    private void Start() {
        // Dapatkan indeks merchant dari MerchantManager
        merchantIndex = MerchantManager.Instance.GetCurrentMerchantIndex();
        rotateIndex = MerchantManager.Instance.GetCurrentRotateIndex();

        InitData(merchantIndex, rotateIndex);
        Debug.Log ("Merchant index ke: " + merchantIndex);
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        konfirmasiPanel.SetActive(false);
        infoRempahCanvas.SetActive(false);

        closeInfoButton.GetComponent<Button>().onClick.AddListener(() => {
            infoRempahCanvas.SetActive(false);
            PersistentManager.Instance.isUIOpen = false;
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        restokDaganganButton.GetComponent<Button>().onClick.AddListener(() => {
            RestokRempah();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        tingkatkanButton.GetComponent<Button>().onClick.AddListener(() => {
            UpgradeKondisi();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        konfirmasiButton.GetComponent<Button>().onClick.AddListener(() => {
            infoRempahCanvas.SetActive(false);
            if (merchantData.levelMerchant == 1) {
                UpgradeRempah2();
            } else if (merchantData.levelMerchant == 2) {
                UpgradeRempah3();
            }
            StartCoroutine(PlayAfterUpgrade());
            PersistentManager.Instance.isUIOpen = false;

            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        batalButton.GetComponent<Button>().onClick.AddListener(() => {
            konfirmasiPanel.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });
    }

    [SerializeField] private bool isDecreaseIndex;
    private void Update() {
        UpdateInfo();
        UpdateGFX();
        UpdatePanelGFX();
        UpdateWindowInfoGFX();

        // if (PersistentManager.Instance.isBossAtMerchant == true) {
        //     isDecreaseIndex = true;
        //     if (isDecreaseIndex) {
        //         merchantIndex--;
        //         isDecreaseIndex = false;
        //     }
        // }
        // ini harusnya bisa buat ngebenerin index out of range
    }

    private void UpdateInfo() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        teksLevelRempah.text = "Pedagang Rempah Level " + merchantData.levelMerchant.ToString();
        teksHargaRempah.text = merchantData.hargaDagangan.ToString("N0") + "K / Pengunjung";
        teksStokDagangan.text = Mathf.Round(merchantData.stokDagangan).ToString() + "/" + merchantData.maxStokDagangan;
        
        if (merchantData.levelMerchant != 3) {
            teksUpLevel.text = "Up Level " + (merchantData.levelMerchant + 1).ToString();
        } else {
            teksUpLevel.text = "Level Maks";
        }

        teksHargaUpgradeRempah.text = merchantData.hargaUpgrade.ToString("N0") + "K";
        teksRempahBatu.text = (PersistentManager.Instance.dataBatu + "/" + merchantData.costUpBatu).ToString();
        teksRempahKayu.text = (PersistentManager.Instance.dataKayu + "/" + merchantData.costUpKayu).ToString();
        teksRempahTanahLiat.text = (PersistentManager.Instance.dataTanahLiat + "/" + merchantData.costUpTanahLiat).ToString();
        teksBatuAkik.text = (PersistentManager.Instance.dataBatuAkik + "/" + merchantData.costUpBatuAkik).ToString();

        teksKonfirm.text = merchantData.hargaUpgrade.ToString("N0") + "K";
        upLevel.text = "Pedagang Rempah Level " + (merchantData.levelMerchant + 1).ToString();
        upMaxStok.text = (merchantData.maxStokDagangan + 5).ToString();
        if (merchantData.levelMerchant == 1) {
            upHarga.text = "180K";
        } else if (merchantData.levelMerchant == 2) {
            upHarga.text = "240K";
        }
        maxStok.text = merchantData.maxStokDagangan.ToString();
        harga.text = merchantData.hargaDagangan.ToString();

        teksPenghasilanRempah.text = merchantData.penghasilanMerchant.ToString("N0");
        teksStokLuar.text = Mathf.Round(merchantData.stokDagangan).ToString() + "/" + merchantData.maxStokDagangan;
    }

    private void UpgradeRempah2() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (PersistentManager.Instance.dataKoin >= merchantData.hargaUpgrade && PersistentManager.Instance.dataBatu >= merchantData.costUpBatu && PersistentManager.Instance.dataKayu >= merchantData.costUpKayu && PersistentManager.Instance.dataTanahLiat >= merchantData.costUpTanahLiat && PersistentManager.Instance.dataBatuAkik >= merchantData.costUpBatuAkik && merchantData.levelMerchant < 3 && merchantData.levelMerchant == 1) {
            PersistentManager.Instance.UpdateKoin(-merchantData.hargaUpgrade);
            PersistentManager.Instance.dataBatu -= merchantData.costUpBatu;
            PersistentManager.Instance.dataKayu -= merchantData.costUpKayu;
            PersistentManager.Instance.dataTanahLiat -= merchantData.costUpTanahLiat;
            PersistentManager.Instance.dataBatuAkik -= merchantData.costUpBatuAkik;

            merchantData.levelMerchant++;
            
            merchantData.hargaDagangan = 180;
            merchantData.maxStokDagangan = 15;

            merchantData.costUpBatu = 12;
            merchantData.costUpKayu = 8;
            merchantData.costUpTanahLiat = 6;
            merchantData.costUpBatuAkik = 1;
            merchantData.hargaUpgrade = 2800;

            konfirmasiPanel.SetActive(false);
        }
    }
            
    private void UpgradeRempah3() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (PersistentManager.Instance.dataKoin >= merchantData.hargaUpgrade && PersistentManager.Instance.dataBatu >= merchantData.costUpBatu && PersistentManager.Instance.dataKayu >= merchantData.costUpKayu && PersistentManager.Instance.dataTanahLiat >= merchantData.costUpTanahLiat && merchantData.levelMerchant < 3 && merchantData.levelMerchant == 2) {
            PersistentManager.Instance.UpdateKoin(-merchantData.hargaUpgrade);
            PersistentManager.Instance.dataBatu -= merchantData.costUpBatu;
            PersistentManager.Instance.dataKayu -= merchantData.costUpKayu;
            PersistentManager.Instance.dataTanahLiat -= merchantData.costUpTanahLiat;
            PersistentManager.Instance.dataBatuAkik -= merchantData.costUpBatuAkik;

            merchantData.levelMerchant++;
        
            merchantData.hargaDagangan = 280;
            merchantData.maxStokDagangan = 20;

            merchantData.costUpBatu = 0;
            merchantData.costUpKayu = 0;
            merchantData.costUpTanahLiat = 0;
            merchantData.costUpBatuAkik = 0;
            merchantData.hargaUpgrade = 0;
            
            tingkatkanButton.SetActive(false);
            maksUpgrade.SetActive(true);

            konfirmasiPanel.SetActive(false);
        }
    }

    private void UpgradeKondisi() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];
        
        bool isBahanCukup = PersistentManager.Instance.dataBatu >= merchantData.costUpBatu 
                        && PersistentManager.Instance.dataKayu >= merchantData.costUpKayu 
                        && PersistentManager.Instance.dataTanahLiat >= merchantData.costUpTanahLiat 
                        && PersistentManager.Instance.dataBatuAkik >= merchantData.costUpBatuAkik;

        bool isKoinCukup = PersistentManager.Instance.dataKoin >= merchantData.hargaUpgrade;

        if (!isBahanCukup) {
            StartCoroutine(PlayLessBahanUpgrade());
        }

        if (!isKoinCukup) {
            StartCoroutine(FeedbackManager.instance.PlayLessKoinUpgrade());
        }

        if (isBahanCukup && isKoinCukup && merchantData.levelMerchant < 3) {
            konfirmasiPanel.SetActive(true);
        }
    }

    [Header("--- LessBahan Upgrade ---")]
    [SerializeField] private GameObject lessBahanUpgrade;
    [SerializeField] private Image lessBahanUpgradeImage;
    [SerializeField] private Sprite[] lessBahanUpgradeFrames;
    private IEnumerator PlayLessBahanUpgrade() {
        // AudioManager.audioManager.PlaySFX(AudioManager.audioManager.coinMinus);
        lessBahanUpgrade.SetActive(true);
        for (int i = 0; i < lessBahanUpgradeFrames.Length; i++) {
            lessBahanUpgradeImage.sprite = lessBahanUpgradeFrames[i];
            yield return new WaitForSeconds(0.055f);
        }
        lessBahanUpgrade.SetActive(false);
    }

    [Header("--- After Upgrade ---")]
    [SerializeField] private GameObject afterUpgrade;
    [SerializeField] private Image afterUpgradeImage;
    [SerializeField] private Sprite[] afterUpgradeFrames;
    private IEnumerator PlayAfterUpgrade() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.placeAndUpgradeBuilding);
        afterUpgrade.SetActive(true);
        for (int i = 0; i < afterUpgradeFrames.Length; i++) {
            afterUpgradeImage.sprite = afterUpgradeFrames[i];
            yield return new WaitForSeconds(0.055f);
        }
        afterUpgrade.SetActive(false);
    }

    private void RestokRempah() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (PersistentManager.Instance.dataStokRempah > 0 && (merchantData.stokDagangan + 1) <= merchantData.maxStokDagangan) {
            merchantData.stokDagangan++;
            PersistentManager.Instance.dataStokRempah--;
        } else if (PersistentManager.Instance.dataStokSayur > 0 && (merchantData.stokDagangan + 1) > merchantData.maxStokDagangan) {
            merchantData.stokDagangan = merchantData.maxStokDagangan;
            PersistentManager.Instance.dataStokSayur--;
        }
    }

    [Header("--- GFX Update ---")]
    [SerializeField] private GameObject merchantGFX;
    [SerializeField] private Sprite GFXKananLevel_1;
    [SerializeField] private Sprite GFXKananLevel_2;
    [SerializeField] private Sprite GFXKananLevel_3;

    [SerializeField] private Sprite GFXKananMatiLevel_1;
    [SerializeField] private Sprite GFXKananMatiLevel_2;
    [SerializeField] private Sprite GFXKananMatiLevel_3;

    [SerializeField] private Sprite GFXKiriLevel_1;
    [SerializeField] private Sprite GFXKiriLevel_2;
    [SerializeField] private Sprite GFXKiriLevel_3;

    [SerializeField] private Sprite GFXKiriMatiLevel_1;
    [SerializeField] private Sprite GFXKiriMatiLevel_2;
    [SerializeField] private Sprite GFXKiriMatiLevel_3;

    [SerializeField] private Sprite GFXBawahLevel_1;
    [SerializeField] private Sprite GFXBawahLevel_2;
    [SerializeField] private Sprite GFXBawahLevel_3;

    [SerializeField] private Sprite GFXBawahMatiLevel_1;
    [SerializeField] private Sprite GFXBawahMatiLevel_2;
    [SerializeField] private Sprite GFXBawahMatiLevel_3;

    private void UpdateGFX() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (merchantData.stokDagangan <= 0) {
            // Cek rotateIndex untuk kondisi stok kosong
            switch (rotateIndex) {
                case 2: // Kanan
                    switch (merchantData.levelMerchant) {
                        case 1:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKananMatiLevel_1;
                            break;
                        case 2:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKananMatiLevel_2;
                            break;
                        case 3:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKananMatiLevel_3;
                            break;
                        default:
                            Debug.LogWarning("Level Merchant tidak valid!");
                            break;
                    }
                    break;

                case 0: // Kiri
                    switch (merchantData.levelMerchant) {
                        case 1:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKiriMatiLevel_1;
                            break;
                        case 2:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKiriMatiLevel_2;
                            break;
                        case 3:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKiriMatiLevel_3;
                            break;
                        default:
                            Debug.LogWarning("Level Merchant tidak valid!");
                            break;
                    }
                    break;

                case 1: // Bawah
                    switch (merchantData.levelMerchant) {
                        case 1:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXBawahMatiLevel_1;
                            break;
                        case 2:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXBawahMatiLevel_2;
                            break;
                        case 3:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXBawahMatiLevel_3;
                            break;
                        default:
                            Debug.LogWarning("Level Merchant tidak valid!");
                            break;
                    }
                    break;

                default:
                    Debug.LogWarning("Rotate Index tidak valid!");
                    break;
            }
        } else {
            // Cek rotateIndex untuk kondisi stok masih ada
            switch (rotateIndex) {
                case 2: // Kanan
                    switch (merchantData.levelMerchant) {
                        case 1:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKananLevel_1;
                            break;
                        case 2:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKananLevel_2;
                            break;
                        case 3:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKananLevel_3;
                            break;
                        default:
                            Debug.LogWarning("Level Merchant tidak valid!");
                            break;
                    }
                    break;

                case 0: // Kiri
                    switch (merchantData.levelMerchant) {
                        case 1:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKiriLevel_1;
                            break;
                        case 2:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKiriLevel_2;
                            break;
                        case 3:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXKiriLevel_3;
                            break;
                        default:
                            Debug.LogWarning("Level Merchant tidak valid!");
                            break;
                    }
                    break;

                case 1: // Bawah
                    switch (merchantData.levelMerchant) {
                        case 1:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXBawahLevel_1;
                            break;
                        case 2:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXBawahLevel_2;
                            break;
                        case 3:
                            merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXBawahLevel_3;
                            break;
                        default:
                            Debug.LogWarning("Level Merchant tidak valid!");
                            break;
                    }
                    break;

                default:
                    Debug.LogWarning("Rotate Index tidak valid!");
                    break;
            }
        }
    }

    [SerializeField] private Sprite panelLevel_2;
    [SerializeField] private Sprite panelLevel_3;
    private void UpdatePanelGFX() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        switch (merchantData.levelMerchant) {
            case 1:
                konfirmasiPanel.GetComponent<Image>().sprite = panelLevel_2;
                break;
            case 2:
                konfirmasiPanel.GetComponent<Image>().sprite = panelLevel_3;
                break;
            case 3:
                break;
            default:
                Debug.LogWarning("Level Merchant tidak valid!");
                break;
        }
    }

    [SerializeField] private GameObject windowInfo;
    [SerializeField] private Sprite windowInfoLevel_1;
    [SerializeField] private Sprite windowInfoLevel_2;
    [SerializeField] private Sprite windowInfoLevel_3;
    private void UpdateWindowInfoGFX() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        switch (merchantData.levelMerchant) {
            case 1:
                windowInfo.GetComponent<Image>().sprite = windowInfoLevel_1;
                break;
            case 2:
                windowInfo.GetComponent<Image>().sprite = windowInfoLevel_2;
                break;
            case 3:
                windowInfo.GetComponent<Image>().sprite = windowInfoLevel_3;
                break;
            default:
                Debug.LogWarning("Level Merchant tidak valid!");
                break;
        }
    }


    [Header("--- Button State ---")]
    [SerializeField] private Sprite normalKonfirm;
    [SerializeField] private Sprite highlightedKonfirm;
    [SerializeField] private Color normalColorKonfirm = Color.black;
    [SerializeField] private Color highlightedColorKonfirm = Color.white;

    public void OnHighlightKonfirm() {
        konfirmasiButton.image.sprite = highlightedKonfirm;
        teksKonfirm.color = highlightedColorKonfirm;
    }

    public void OnUnhighlightKonfirm() {
        konfirmasiButton.image.sprite = normalKonfirm;
        teksKonfirm.color = normalColorKonfirm;
    }

    [SerializeField] private Sprite normalBatal;
    [SerializeField] private Sprite highlightedBatal;

    public void OnHighlightBatal() {
        batalButton.image.sprite = highlightedBatal;
    }

    public void OnUnhighlightBatal() {
        batalButton.image.sprite = normalBatal;
    }

    public void InitData(int indexMerchant, int indexRotate) {
        merchantIndex = indexMerchant;
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];
        merchantData.merchantObject = this.gameObject;
        merchantData.rotateIndex = MerchantManager.Instance.GetCurrentRotateIndex();
        merchantData.levelMerchant = 1;
        merchantData.hargaDagangan = 125f;
        merchantData.stokDagangan = 10f;
        merchantData.maxStokDagangan = 10f;
        merchantData.costUpBatu = 6;
        merchantData.costUpKayu = 4;
        merchantData.costUpTanahLiat = 3;
        merchantData.costUpBatuAkik = 1;
        merchantData.hargaUpgrade = 1800;
    }

    private void ScanAstar() {
        GameObject aStarObject = GameObject.Find("A_Star");

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
