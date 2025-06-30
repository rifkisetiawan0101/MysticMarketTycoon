using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    public static TutorialManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private void Start() {
        if (!isTutorialJalanPlayed) {
            StartCoroutine(PlayTutorialJalan());
        } else {
            StopCoroutine(PlayTutorialJalan());
        }

        buttonTutorMerchant.GetComponent<Button>().onClick.AddListener(() => {
            // on Click ShopUI.ShowMerchant di Inspector
            overlayMerchant.SetActive(false);
            isMerchantShow = true;
            buttonTutorMerchant.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonTutorFurnitur.GetComponent<Button>().onClick.AddListener(() => {
            // on Click ShopUI.ShowFurnitur di Inspector
            overlayFurnitur.SetActive(false);
            isFurniturShow = true;
            buttonTutorFurnitur.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonTutorSpesial.GetComponent<Button>().onClick.AddListener(() => {
            // on Click ShopUI.ShowSpesial di Inspector
            overlaySpesial.SetActive(false);
            isSpesialShow = true;
            buttonTutorSpesial.SetActive(false);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        blokirShop.SetActive(true);
        blokirMenu.SetActive(true);
        blokirPenghargaan.SetActive(true);
        blokirButtonMerchant.SetActive(true);
        blokirButtonFurnitur.SetActive(true);
        blokirButtonSpesial.SetActive(true);
        klikKiriSkip.SetActive(false);
        animasiKlikJalan.SetActive(false);
        
    

        tutorialUto.GetComponent<Image>().sprite = tutorialUtoSprites[currentIndex];
        buttonMengerti.gameObject.SetActive(false);
        buttonSebelumnya.onClick.AddListener(PreviousImage);
        buttonSelanjutnya.onClick.AddListener(NextImage);
        buttonMengerti.onClick.AddListener(CloseTutorialUto);

        buttonAkikMengerti.onClick.AddListener(CloseTutorialAkik);
        buttonUpMengerti.onClick.AddListener(CloseTutorialUpgrade);

        if (!isTutorialRempahPlayed) {
            StartCoroutine(PlayTutorialRempah());
        } else {
            StopCoroutine(PlayTutorialRempah());
        }

        // buttonBeliRempah.GetComponent<Button>().onClick.AddListener(() => MerchantSelectUI.Instance.OnClickTutorialBeliRempah(buttonBeliRempah));
        // buttonTambahPedagang.GetComponent<Button>().onClick.AddListener(() => MerchantSelectUI.Instance.OnClickTutorialBeliRempah(buttonTambahPedagang));

        buttonDialogSelanjutnya.GetComponent<Button>().onClick.AddListener (() => {
            dialogIndex++;
            dialogUrban.GetComponent<Image>().sprite = dialogUrbanSprites[dialogIndex];
            
            if (dialogIndex == 2) {
                buttonDialogSelanjutnya.SetActive(false);
                isDialogFinish = true;
            }
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonMonologSelanjutnya.GetComponent<Button>().onClick.AddListener (() => {
            monologIndex++;
            monologUrban.GetComponent<Image>().sprite = monologUrbanSprites[monologIndex];
            
            if (monologIndex == 2) {
                buttonMonologSelanjutnya.SetActive(false);
                isMonologFinish = true;
            }
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        buttonOverSelanjutnya.GetComponent<Button>().onClick.AddListener (() => {
            overIndex++;
            monologGameOver.GetComponent<Image>().sprite = monologOverSprites[overIndex];
            
            if (overIndex == 1) {
                buttonOverSelanjutnya.SetActive(false);
                isOverFinish = true;
            }
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        });

        StartCoroutine(AnimasiPanah());
        StartCoroutine(AnimasiBeliStok());
    }

    [SerializeField] private GameObject klikKiriSkip;
    
    [Header("--- Animasi --- ")]
    private float delay_18 = 0.055f;
    [SerializeField] GameObject animasiKlikJalan;
    [SerializeField] private Sprite[] animasiKlikJalanFrames;
    private IEnumerator AnimasiKlikJalan() {
        animasiKlikJalan.SetActive(true);
        while (true) {
            for (int i = 0; i < animasiKlikJalanFrames.Length; i++) {
                animasiKlikJalan.GetComponent<Image>().sprite = animasiKlikJalanFrames[i];
                yield return new WaitForSeconds(delay_18);
            }
        }
    }

    [SerializeField] private GameObject animasiKlikKiri;
    [SerializeField] private Sprite[] animasiKlikKiriFrames;
    private IEnumerator AnimasiKlikKiri() {
        while (true) {
            for (int i = 0; i < animasiKlikKiriFrames.Length; i++) {
                animasiKlikKiri.GetComponent<Image>().sprite = animasiKlikKiriFrames[i];
                yield return new WaitForSeconds(delay_18);
            }
        }
    }

    [SerializeField] private GameObject panahMerchant;
    [SerializeField] private GameObject panahFurnitur;
    [SerializeField] private GameObject panahSpesial;
    [SerializeField] private Sprite[] panahFrames;
    private IEnumerator AnimasiPanah() {
        while (true) {
            for (int i = 0; i < panahFrames.Length; i++) {
                panahMerchant.GetComponent<Image>().sprite = panahFrames[i];
                panahFurnitur.GetComponent<Image>().sprite = panahFrames[i];
                panahSpesial.GetComponent<Image>().sprite = panahFrames[i];
                yield return new WaitForSeconds(delay_18);
            }
        }
    }

    [SerializeField] private GameObject animasiBeliStok;
    [SerializeField] private Sprite[] animasiBeliStokFrames;
    private IEnumerator AnimasiBeliStok() {
        while (true) {
            for (int i = 0; i < animasiBeliStokFrames.Length; i++) {
                animasiBeliStok.GetComponent<Image>().sprite = animasiBeliStokFrames[i];
                yield return new WaitForSeconds(delay_18);
            }
        }
    }

    [Header("1. Tutorial Jalan")]
    [SerializeField] private GameObject tutorialJalan;
    [SerializeField] private bool isTutorialJalanPlayed = false;
    
    private IEnumerator PlayTutorialJalan() {
        yield return new WaitForSeconds(1.1f);
        tutorialJalan.SetActive(true);
        StartCoroutine(AnimasiKlikJalan());

        yield return new WaitUntil (() => Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject());
        
        tutorialJalan.SetActive(false);
        StopCoroutine(AnimasiKlikJalan());
        animasiKlikJalan.SetActive(false);
        isTutorialJalanPlayed = true;

        if (isTutorialJalanPlayed && !isTutorialMerchantPlayed) {
            StartCoroutine(PlayTutorialMerchant());
        } else {
            StopCoroutine(PlayTutorialMerchant());
        }
    }

    [Header("2. Tutorial Merchant")]
    [SerializeField] private GameObject tutorialMerchant;
    [SerializeField] private GameObject blokirShop;
    [SerializeField] private GameObject overlayMerchant; // isinya button dan DiTanganPedagang
    [SerializeField] private GameObject buttonTutorMerchant;
    [SerializeField] private bool isMerchantShow = false;
    [SerializeField] private GameObject pilihTokoPertama;
    [SerializeField] private GameObject pilihMerchant;
    [SerializeField] public bool isPilihToko = false;
    [SerializeField] private GameObject taruhMerchant;
    [SerializeField] public bool isTaruhMerchant = false;

    [SerializeField] private GameObject blokirButtonMerchant;

    [SerializeField] private bool isTutorialMerchantPlayed = false;
    private IEnumerator PlayTutorialMerchant() {
        overlayMerchant.SetActive(false);
        buttonTutorMerchant.SetActive(false);
        pilihTokoPertama.SetActive(false);
        taruhMerchant.SetActive(false);
        blokirWorld.SetActive(true);

        yield return new WaitUntil(() => PlayerMovementNew.isMoving == false);
        tutorialMerchant.SetActive(true);
        overlayMerchant.SetActive(true);
        buttonTutorMerchant.SetActive(true);
        blokirButtonMerchant.SetActive(false);
        blokirShop.SetActive(false);

        yield return new WaitUntil(() => isMerchantShow == true);
        yield return new WaitForSeconds(0.5f);
        pilihTokoPertama.SetActive(true);
        pilihMerchant.SetActive(true);

        yield return new WaitUntil(() => isPilihToko == true);
        pilihTokoPertama.SetActive(false);
        pilihMerchant.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        taruhMerchant.SetActive(true);
        blokirWorld.SetActive(false);

        yield return new WaitUntil(() => isTaruhMerchant == true);
        taruhMerchant.SetActive(false);
        blokirWorld.SetActive(true);
        blokirShop.SetActive(true);

        tutorialMerchant.SetActive(false);
        isTutorialMerchantPlayed = true;

        if (isTutorialMerchantPlayed && !isTutorialFurniturStatuePlayed) {
            StartCoroutine(PlayTutorialFurniturStatue());
        } else {
            StopCoroutine(PlayTutorialFurniturStatue());
        }
    }

    [Header("3. Tutorial Furnitur dan Statue")]
    [SerializeField] private GameObject tutorialFurniturStatue;
    [SerializeField] private GameObject blokirWorld;
    [SerializeField] private GameObject overlayFurnitur;
    [SerializeField] private GameObject buttonTutorFurnitur;
    [SerializeField] private bool isFurniturShow = false;
    [SerializeField] private GameObject overlaySpesial;
    [SerializeField] private GameObject buttonTutorSpesial;
    [SerializeField] private bool isSpesialShow = false;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private GameObject wualahCik;
    [SerializeField] public bool isTutorialFurniturStatuePlayed = false;

    [SerializeField] private GameObject blokirButtonFurnitur;
    [SerializeField] private GameObject blokirButtonSpesial;
    private IEnumerator PlayTutorialFurniturStatue() {
        blokirButtonMerchant.SetActive(true);
        blokirButtonFurnitur.SetActive(true);
        blokirButtonSpesial.SetActive(true);
        wualahCik.SetActive(false);

        yield return new WaitForSeconds(3f);
        tutorialFurniturStatue.SetActive(true);
        overlayFurnitur.SetActive(true);
        buttonTutorFurnitur.SetActive(true);
        blokirButtonFurnitur.SetActive(false);

        yield return new WaitUntil(() => isFurniturShow == true);
        yield return new WaitForSeconds(0.5f);
        overlaySpesial.SetActive(true);
        blokirButtonFurnitur.SetActive(true);
        buttonTutorSpesial.SetActive(true);
        blokirButtonSpesial.SetActive(false);

        yield return new WaitUntil(() => isSpesialShow == true);
        blokirWorld.SetActive(false);
        yield return new WaitForSeconds(3f);
        shopUI.CloseShopUI();
        wualahCik.SetActive(true);
        blokirWorld.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        klikKiriSkip.SetActive(true);
        StartCoroutine(AnimasiKlikKiri());

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        klikKiriSkip.SetActive(false);
        StopCoroutine(AnimasiKlikKiri());
        
        blokirButtonMerchant.SetActive(false);
        blokirButtonFurnitur.SetActive(false);
        blokirButtonSpesial.SetActive(false);
        blokirWorld.SetActive(false);
        blokirShop.SetActive(false);
        tutorialFurniturStatue.SetActive(false);
        isTutorialFurniturStatuePlayed = true;

        if (isTutorialFurniturStatuePlayed && !isTutorialHantuPlayed) {
            StartCoroutine(PlayTutorialHantu());
        } else {
            StopCoroutine(PlayTutorialHantu());
        }
    }

    [Header("4. Tutorial Hantu")]
    [SerializeField] private GameObject tutorialHantu;
    [SerializeField] private GameObject klikKoin;
    public bool isNpcSpawn = false;
    public bool isKoinActive = false;
    [SerializeField] private bool isTutorialHantuPlayed = false;

    private IEnumerator PlayTutorialHantu() {
        klikKoin.SetActive(false);
        yield return new WaitUntil(() => isNpcSpawn == true);
        yield return new WaitForSeconds(5f);
        tutorialHantu.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        klikKiriSkip.SetActive(true);
        StartCoroutine(AnimasiKlikKiri());
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        klikKiriSkip.SetActive(false);
        StopCoroutine(AnimasiKlikKiri());
        
        tutorialHantu.SetActive(false);

        yield return new WaitUntil(() => isKoinActive == true);
        klikKoin.SetActive(true);

        yield return new WaitUntil(() => isKoinActive == false);
        klikKoin.SetActive(false);

        isTutorialHantuPlayed = true;

        if (isTutorialHantuPlayed && !isTutorialPenghargaanPlayed) {
            StartCoroutine(PlayTutorialPenghargaan());
        } else {
            StopCoroutine(PlayTutorialPenghargaan());
        }
    }

    [Header("5. Tutorial Penghargaan")]
    [SerializeField] private GameObject tutorialPenghargaan;
    [SerializeField] private GameObject blokirPenghargaan;
    [SerializeField] private GameObject overlayPenghargaan;
    public bool isTimerStart = false;
    public bool isOverlayPenghargaan = false;
    public bool isPenghargaanOpen = false;
    [SerializeField] private bool isTutorialPenghargaanPlayed = false;

    private IEnumerator PlayTutorialPenghargaan() {
        isTimerStart = true;
        yield return new WaitForSeconds(5f);
        tutorialPenghargaan.SetActive(true);
        blokirPenghargaan.SetActive(false);
        blokirMenu.SetActive(false);

        yield return new WaitUntil (() => isOverlayPenghargaan == true);
        overlayPenghargaan.SetActive(false);

        yield return new WaitUntil (() => isPenghargaanOpen == true);
        tutorialPenghargaan.SetActive(false);
        isTutorialPenghargaanPlayed = true;

        if (isTutorialPenghargaanPlayed && !isTutorialWaktuPlayed) {
            StartCoroutine(PlayTutorialWaktu());
        } else {
            StopCoroutine(PlayTutorialWaktu());
        }
    }

    [Header("6. Tutorial Waktu")]
    [SerializeField] private GameObject tutorialWaktu;
    [SerializeField] private GameObject blokirMenu;
    [SerializeField] private Timer timer;
    // is timer start di penghargaan
    private bool isTimerZeroZero = false;
    private void OnEnable() {
        timer.OnTutorialTimer += HandleTimerZeroZero;
        timer.OnDialogUrban += StartHandleDialogUrban;
        timer.OnMonologUrban += StartHandleMonologUrban;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void HandleTimerZeroZero() {
        tutorialWaktu.SetActive(true);
        isTimerZeroZero = true;
    }

    [SerializeField] public bool isTutorialWaktuPlayed = false;

    private IEnumerator PlayTutorialWaktu() {
        yield return new WaitUntil(() => isTimerZeroZero == true);
        yield return new WaitForSeconds(1.5f);
        klikKiriSkip.SetActive(true);
        StartCoroutine(AnimasiKlikKiri());
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        klikKiriSkip.SetActive(false);
        StopCoroutine(AnimasiKlikKiri());
        
        tutorialWaktu.SetActive(false);
        isTutorialWaktuPlayed = true;
    }

    public void StartTutorialUto() {
        if (isTutorialWaktuPlayed && !isTutorialUtoPlayed) {
            StartCoroutine(PlayTutorialUto());
        } else {
            StopCoroutine(PlayTutorialUto());
        }
    }

    [Header("7. Tutorial Uto")]
    [SerializeField] private GameObject tutorialUto;
    [SerializeField] private Sprite[] tutorialUtoSprites; // Array untuk menyimpan sprites
    public bool isUtoSpawn = false;
    [SerializeField] private Button buttonSebelumnya;
    [SerializeField] private Button buttonSelanjutnya;
    [SerializeField] private Button buttonMengerti;
    private int currentIndex = 0; // Indeks saat ini untuk sprite yang ditampilkan

    [SerializeField] public bool isTutorialUtoPlayed = false;

    private IEnumerator PlayTutorialUto() {
        yield return new WaitUntil(() => isUtoSpawn == true);
        yield return new WaitForSeconds(0.5f);
        tutorialUto.SetActive(true);
        Time.timeScale = 0;
    }

    private void PreviousImage() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        if (currentIndex > 0) {
            currentIndex--; // Decrement indeks untuk kembali ke gambar sebelumnya
            tutorialUto.GetComponent<Image>().sprite = tutorialUtoSprites[currentIndex];
            UpdateButtonInteractable();

            // Jika kembali ke index 1 atau 0, nonaktifkan button Mengerti
            if (currentIndex < tutorialUtoSprites.Length - 1) {
                buttonMengerti.gameObject.SetActive(false);
            }
        }
    }

    private void NextImage() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        if (currentIndex < tutorialUtoSprites.Length - 1) {
            currentIndex++; // Increment indeks untuk menuju ke gambar berikutnya
            tutorialUto.GetComponent<Image>().sprite = tutorialUtoSprites[currentIndex];
            UpdateButtonInteractable();

            // Jika mencapai gambar terakhir (index 2), aktifkan button Mengerti
            if (currentIndex == tutorialUtoSprites.Length - 1) {
                buttonMengerti.gameObject.SetActive(true);
            }
        }
    }

    private void CloseTutorialUto() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        Time.timeScale = 1;
        tutorialUto.SetActive(false);
        isTutorialUtoPlayed = true;
    }

    private void UpdateButtonInteractable() {
        buttonSebelumnya.interactable = currentIndex > 0;
        buttonSelanjutnya.interactable = currentIndex < tutorialUtoSprites.Length - 1;
    }

    public void StartTutorialAkik() {
        if (isTutorialUtoPlayed && !isTutorialAkikPlayed) {
            StartCoroutine(PlayTutorialAkik());
        } else {
            StopCoroutine(PlayTutorialAkik());
        }
    }

    [Header("8. Tutorial Akik")]
    [SerializeField] private GameObject tutorialAkik;
    [SerializeField] private Button buttonAkikMengerti;
    // [SerializeField] private GameObject blokirButtonAkik;
    [SerializeField] private bool isTutorialAkikPlayed = false;
    
    private IEnumerator PlayTutorialAkik() {
        // blokirButtonAkik.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialAkik.SetActive(true);
        Time.timeScale = 0;
    }

    private void CloseTutorialAkik() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        Time.timeScale = 1;
        tutorialAkik.SetActive(false);
        // blokirButtonAkik.SetActive(false);
        isTutorialAkikPlayed = true;
    }

    public void StartTutorialUpgrade() {
        if (isTutorialAkikPlayed && !isTutorialUpgradePlayed) {
            StartCoroutine(PlayTutorialUpgrade());
        } else {
            StopCoroutine(PlayTutorialUpgrade());
        }
    }

    [Header("9. Tutorial Upgrade")]
    [SerializeField] private GameObject tutorialUpgrade;
    [SerializeField] private Button buttonUpMengerti;
    [SerializeField] private bool isTutorialUpgradePlayed = false;
    
    private IEnumerator PlayTutorialUpgrade() {
        yield return new WaitForSeconds(2f);
        tutorialUpgrade.SetActive(true);
        Time.timeScale = 0;
    }

    private void CloseTutorialUpgrade() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        Time.timeScale = 1;
        tutorialUpgrade.SetActive(false);
        isTutorialUpgradePlayed = true;
    }
    
    public void StartTutorialInfo() {
        if (isTutorialUpgradePlayed && !isTutorialInfoPlayed) {
            StartCoroutine(PlayTutorialInfo());
        } else {
            StopCoroutine(PlayTutorialInfo());
        }
    }

    [Header("Tutorial Info Merchant")]
    [SerializeField] private GameObject upgradeTokomu;
    [SerializeField] private GameObject stokMenipis;
    public bool isTutorialInfoPlayed = false;
    private IEnumerator PlayTutorialInfo() {
        upgradeTokomu.SetActive(true);
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil (() => Input.GetMouseButtonDown(0));
        upgradeTokomu.SetActive(false);
        stokMenipis.SetActive(true);
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil (() => Input.GetMouseButtonDown(0));
        stokMenipis.SetActive(false);
        isTutorialInfoPlayed = true;
    }

    public void StartTutorialRestok() {
        if (isTutorialUpgradePlayed && !isTutorialRestokPlayed && isCutPagiPertama) {
            StartCoroutine(PlayTutorialRestok());
        } else {
            StopCoroutine(PlayTutorialRestok());
        }
    }

    [Header("10. Tutorial Restok")]
    [SerializeField] private GameObject tutorialRestok;
    [SerializeField] private GameObject lihatMobil;
    [SerializeField] private GameObject membeliStok;
    [SerializeField] private GameObject blokirLahan;
    [SerializeField] private GameObject buttonTutorMulaiPasar;

    public bool isCutPagiPertama = false;
    public bool isRestokOpen = false;
    [SerializeField] public bool isTutorialRestokPlayed = false;

    private IEnumerator PlayTutorialRestok() {
        tutorialRestok.SetActive(true);
        lihatMobil.SetActive(true);
        blokirLahan.SetActive(true);
        buttonTutorMulaiPasar.SetActive(true);
        buttonTutorMulaiPasar.GetComponent<Button>().interactable = false;
        ScanAstar();
        
        yield return new WaitUntil(() => isRestokOpen == true);
        lihatMobil.SetActive(false);
        membeliStok.SetActive(true);
        blokirLahan.SetActive(false);
        ScanAstar();

        yield return new WaitUntil(() => isRestokOpen == false);
        membeliStok.SetActive(false);
        isTutorialRestokPlayed = true;

        if (isTutorialRestokPlayed && !isTutorialItemPlayed) {
            StartCoroutine(PlayTutorialItem());
        } else {
            StopCoroutine(PlayTutorialItem());
        }
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

    [Header("11. Tutorial Item")]
    [SerializeField] private GameObject tutorialItem;
    [SerializeField] private GameObject lihatLahanmu;
    [SerializeField] private GameObject klikDanTahan;
    [SerializeField] private GameObject membukaPasar;
    public bool isNearItem = false;
    public bool isItemCollected = false;
    [SerializeField] private bool isTutorialItemPlayed = false;

    private IEnumerator PlayTutorialItem() {
        tutorialItem.SetActive(true);
        lihatLahanmu.SetActive(true);
        klikDanTahan.SetActive(false);
        membukaPasar.SetActive(false);

        yield return new WaitUntil(() => isNearItem == true);
        lihatLahanmu.SetActive(false);
        klikDanTahan.SetActive(true);

        yield return new WaitUntil(() => isItemCollected == true);
        klikDanTahan.SetActive(false);

        yield return new WaitForSeconds(8f);
        membukaPasar.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        buttonTutorMulaiPasar.GetComponent<Button>().interactable = true;
        buttonTutorMulaiPasar.GetComponent<Image>().raycastTarget = false;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));       
        buttonTutorMulaiPasar.SetActive(false);
        membukaPasar.SetActive(false);
        tutorialItem.SetActive(false);
        isTutorialItemPlayed = true;
    }

    [Header("Tutorial Beli Rempah")]
    [SerializeField] private GameObject tutorialBeliRempah;
    [SerializeField] private GameObject beliRempah;
    [SerializeField] private GameObject gasBeli;
    [SerializeField] public bool isBeliRempah;
    [SerializeField] private bool isTutorialRempahPlayed;
    private IEnumerator PlayTutorialRempah() {
        yield return new WaitUntil(() => PersistentManager.Instance.dataKoin >= 1250);
        yield return new WaitUntil(() => isTutorialUpgradePlayed == true);
        
        if (PersistentManager.Instance.isNowMalam && timer.elapsedTime <= 130) {
            yield return new WaitForSeconds(1f);
            tutorialBeliRempah.SetActive(true);
            beliRempah.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            klikKiriSkip.SetActive(true);
            StartCoroutine(AnimasiKlikKiri());

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            klikKiriSkip.SetActive(false);
            StopCoroutine(AnimasiKlikKiri());
            
            beliRempah.SetActive(false);
            gasBeli.SetActive(true);
            shopUI.ShowMerchantUI();

            yield return new WaitUntil (() => isBeliRempah == true);
            gasBeli.SetActive(false);
            tutorialBeliRempah.SetActive(false);
            isTutorialRempahPlayed = true;
        }
    }

    [Header("Dialog Urban")]
    [SerializeField] private GameObject dialogUrban;
    [SerializeField] private Sprite[] dialogUrbanSprites;
    [SerializeField] private GameObject buttonDialogSelanjutnya;
    [SerializeField] private int dialogIndex;
    [SerializeField] private bool isDialogFinish;
    [SerializeField] private bool isDialogUrbanPlayed;
    private IEnumerator HandleDialogUrban() {
        dialogUrban.SetActive(true);

        yield return new WaitUntil(() => isDialogFinish == true);
        yield return new WaitForSeconds(1.5f);
        klikKiriSkip.SetActive(true);
        StartCoroutine(AnimasiKlikKiri());

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        klikKiriSkip.SetActive(false);
        StopCoroutine(AnimasiKlikKiri());
        
        dialogUrban.SetActive(false);
        isDialogUrbanPlayed = true;
    }

    private void StartHandleDialogUrban() {
        if (!isDialogUrbanPlayed) {
            StartCoroutine(HandleDialogUrban());
        } else {
            StopCoroutine(HandleDialogUrban());
        }
    }

    [Header("Monolog Urban")]
    [SerializeField] private GameObject monologUrban;
    [SerializeField] private Sprite[] monologUrbanSprites;
    [SerializeField] private GameObject buttonMonologSelanjutnya;
    private int monologIndex;
    [SerializeField] private bool isMonologFinish;
    [SerializeField] private bool isMonologUrbanPlayed;
    private IEnumerator HandleMonologUrban() {
        monologUrban.SetActive(true);

        yield return new WaitUntil(() => isMonologFinish == true);
        yield return new WaitForSeconds(0.75f);
        klikKiriSkip.SetActive(true);
        StartCoroutine(AnimasiKlikKiri());

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        klikKiriSkip.SetActive(false);
        StopCoroutine(AnimasiKlikKiri());
        
        monologUrban.SetActive(false);
        isMonologUrbanPlayed = true;
    }

    private void StartHandleMonologUrban() {
        if (!isMonologUrbanPlayed) {
            StartCoroutine(HandleMonologUrban());
        } else {
            StopCoroutine(HandleMonologUrban());
        }
    }

    [Header("Tutorial Goa")]
    [SerializeField] private GameObject monologGameOver;
    [SerializeField] private Sprite[] monologOverSprites;
    [SerializeField] private GameObject buttonOverSelanjutnya;
    [SerializeField] private GameObject jejakKaki;
    [SerializeField] private GameObject goaOverlay;
    private int overIndex;
    private bool isOverFinish;
    [SerializeField] private bool isMonologGameOverPlayed;
    private IEnumerator HandleMonologGameOver() {
        yield return new WaitForSeconds(2f);
        monologGameOver.SetActive(true);
        jejakKaki.SetActive(true);
        goaOverlay.SetActive(true);

        yield return new WaitUntil(() => isOverFinish == true);
        yield return new WaitForSeconds(1.5f);
        klikKiriSkip.SetActive(true);
        StartCoroutine(AnimasiKlikKiri());

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        // monologGameOver.SetActive(false);
        klikKiriSkip.SetActive(false);
        StopCoroutine(AnimasiKlikKiri());
        goaOverlay.SetActive(false);

        isMonologGameOverPlayed = true;
    }

    public void StartHandleMonologGameOver() {
        if (!isMonologGameOverPlayed) {
            StartCoroutine(HandleMonologGameOver());
        } else {
            StopCoroutine(HandleMonologGameOver());
        }
    }

    [Header("Button States")]
    // di inspektor
    [SerializeField] private Sprite normalSebelumnya;
    [SerializeField] private Sprite highlightedSebelumnya;
    [SerializeField] private Sprite normalSelanjutnya;
    [SerializeField] private Sprite highlightedSelanjutnya;
    [SerializeField] private Sprite normalMengerti;
    [SerializeField] private Sprite highlightedMengerti;
    [SerializeField] private Sprite normalTutorMulaiPasar;
    [SerializeField] private Sprite highlightedTutorMulaiPasar;
    [SerializeField] private Sprite normalDialogSelanjutnya;
    [SerializeField] private Sprite highlightedDialogSelanjutnya;
    [SerializeField] private Sprite normalMonologSelanjutnya;
    [SerializeField] private Sprite highlightedMonologSelanjutnya;
    [SerializeField] private Sprite normalOverSelanjutnya;
    [SerializeField] private Sprite highlightedOverSelanjutnya;
    public void OnEnterSebelumnya() {
        buttonSebelumnya.image.sprite = highlightedSebelumnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSebelumnya() {
        buttonSebelumnya.image.sprite = normalSebelumnya;
    }

    public void OnEnterSelanjutnya() {
        buttonSelanjutnya.image.sprite = highlightedSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitSelanjutnya() {
        buttonSelanjutnya.image.sprite = normalSelanjutnya;
    }

    public void OnEnterMengerti() {
        buttonMengerti.image.sprite = highlightedMengerti;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMengerti() {
        buttonMengerti.image.sprite = normalMengerti;
    }

    public void OnEnterAkikMengerti() {
        buttonAkikMengerti.image.sprite = highlightedMengerti;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitAkikMengerti() {
        buttonAkikMengerti.image.sprite = normalMengerti;
    }

    public void OnEnterUpMengerti() {
        buttonUpMengerti.image.sprite = highlightedMengerti;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitUpMengerti() {
        buttonUpMengerti.image.sprite = normalMengerti;
    }

    public void OnEnterTutorMulaiPasar() {
        buttonTutorMulaiPasar.GetComponent<Image>().sprite = highlightedTutorMulaiPasar;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitTutorMulaiPasar() {
        buttonTutorMulaiPasar.GetComponent<Image>().sprite = normalTutorMulaiPasar;
    }

    public void OnEnterDialogSelanjutnya() {
        buttonDialogSelanjutnya.GetComponent<Image>().sprite = highlightedDialogSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitDialogSelanjutnya() {
        buttonDialogSelanjutnya.GetComponent<Image>().sprite = normalDialogSelanjutnya;
    }

    public void OnEnterMonologSelanjutnya() {
        buttonMonologSelanjutnya.GetComponent<Image>().sprite = highlightedMonologSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitMonologSelanjutnya() {
        buttonMonologSelanjutnya.GetComponent<Image>().sprite = normalMonologSelanjutnya;
    }

    public void OnEnterOverSelanjutnya() {
        buttonOverSelanjutnya.GetComponent<Image>().sprite = highlightedOverSelanjutnya;
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    public void OnExitOverSelanjutnya() {
        buttonOverSelanjutnya.GetComponent<Image>().sprite = normalOverSelanjutnya;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        goaOverlay = GameObject.Find("Map/Goa/Canvas/OverlayGoa");
        jejakKaki = GameObject.Find("Map/Goa/JejakKaki");
    }
}
