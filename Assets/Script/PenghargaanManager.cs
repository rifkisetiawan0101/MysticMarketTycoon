using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PenghargaanManager : MonoBehaviour {
    [SerializeField] private SpesialSelectUI spesialSelectUI;
    // [SerializeField] private bool notifPenghargaan.SetActive(false);

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private void Update() {
        if (PersistentManager.Instance.isCuanDiHutanClaimed == true) {
            buttonCollectPenghargaan_4.SetActive(false);
            notifBool_4 = false;
        }

        if (PersistentManager.Instance.isPebisnisGhaibClaimed == true) {
            buttonCollectPenghargaan_10.SetActive(false);
            notifBool_10 = false;
        }

        if (notifBool_1 == true || notifBool_2 == true || notifBool_3 == true || notifBool_4 == true || notifBool_5 == true || notifBool_6 == true || notifBool_7 == true || notifBool_8 == true || notifBool_9 == true || notifBool_10 == true) {
            notifPenghargaan.SetActive(true);
        } else if (notifBool_1 == false && notifBool_2 == false && notifBool_3 == false && notifBool_4 == false && notifBool_5 == false && notifBool_6 == false && notifBool_7 == false && notifBool_8 == false && notifBool_9 == false && notifBool_10 == false) {
            notifPenghargaan.SetActive(false);
        }
    }
    
    private void OnEnable() { // JANGAN PERNAH DIUBAH KE UPDATE
        PersistentManager.OnTotalMerchantChanged += AwalEkonomi;
        PersistentManager.OnTotalMerchantChanged += ProyekBesar;
    
        MerchantManager.OnJuraganPasar += JuraganPasar;
        
        PersistentManager.OnTotalKoinChanged += CuanDiHutan;

        PersistentManager.OnDayCounterChanged += HinggaTerbitFajar;

        PersistentManager.OnUtoDefeated += AkuDukun;

        PersistentManager.OnTotalCollectableChanged += PengepulAlam;
        PersistentManager.OnTotalCollectableChanged += KuliSakti;

        PersistentManager.OnNightCounterChanged += MalamKliwon;

        PersistentManager.OnTotalKoinChanged += PebisnisGhaib;
    }

    private void Start() {
        StartCoroutine(AnimasiPenghargaan());
    }

    public static PenghargaanManager Instance { get; private set; }
    private float delay_18 = 0.055f;
    [SerializeField] private GameObject notifPenghargaan;
    [SerializeField] private Sprite[] notifPenghargaanFrames;
    private IEnumerator AnimasiPenghargaan() {
        while (true) {
            for (int i = 0; i < notifPenghargaanFrames.Length; i++) {
                notifPenghargaan.GetComponent<Image>().sprite = notifPenghargaanFrames[i];
                yield return new WaitForSeconds(delay_18);
            }
        }
    }

    [Header("--- 1. Memiliki 2 pedagang pertama ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_1;
    [SerializeField] private GameObject dateTextGO_1;
    [SerializeField] private GameObject notif_1;
    [SerializeField] private bool notifBool_1;

    private void AwalEkonomi() {
        if (PersistentManager.Instance.dataTotalMerchant == 2) {
            if (PersistentManager.Instance.isAwalEkonomiPlayed == false) {
                StartCoroutine(PlayNotif_1());
            }
            PersistentManager.Instance.isAwalEkonomiPlayed = true;
            notifBool_1 = true;

            buttonCollectPenghargaan_1.SetActive(true);
            buttonCollectPenghargaan_1.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isAwalEkonomiClaimed == false) {
                    notifBool_1 = false;
                    buttonCollectPenghargaan_1.SetActive(false);
                    dateTextGO_1.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_1.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                    PersistentManager.Instance.UpdateKoin(200f);
                    PersistentManager.Instance.isAwalEkonomiClaimed = true;

                    spesialSelectUI.terkunciButtonList[0].gameObject.SetActive(false);

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isAwalEkonomiClaimed == true) {
                buttonCollectPenghargaan_1.SetActive(false);
                notifBool_1 = false;
            } else {
                Debug.LogError("List terkunciButtonList kosong atau tidak memiliki elemen di index 0!");
            }
        }
    }

    private IEnumerator PlayNotif_1() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_1.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_1.SetActive(false);
    }

    [Header("--- 2. Memiliki 4 pedagang pertama ---")]
    // [SerializeField] private Image ceklis_2;
    [SerializeField] private GameObject buttonCollectPenghargaan_2;
    [SerializeField] private GameObject dateTextGO_2;
    [SerializeField] private GameObject notif_2;
    [SerializeField] private bool notifBool_2;

    private void ProyekBesar() {
        if (PersistentManager.Instance.dataTotalMerchant == 4) {
            if (PersistentManager.Instance.isProyekBesarPlayed == false) {
                StartCoroutine(PlayNotif_2());
            }
            PersistentManager.Instance.isProyekBesarPlayed = true;
            notifBool_2 = true;

            buttonCollectPenghargaan_2.SetActive(true);
            buttonCollectPenghargaan_2.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isProyekBesarClaimed == false) {
                    notifBool_2 = false;
                    buttonCollectPenghargaan_2.SetActive(false);
                    dateTextGO_2.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_2.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                    PersistentManager.Instance.UpdateKoin(400f);
                    PersistentManager.Instance.isProyekBesarClaimed = true;

                    spesialSelectUI.terkunciButtonList[1].gameObject.SetActive(false);

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isProyekBesarClaimed == true) {
                buttonCollectPenghargaan_2.SetActive(false);
                notifBool_2 = false;
            }
        }
    }

    private IEnumerator PlayNotif_2() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_2.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_2.SetActive(false);
    }

    [Header("--- 3. Memiliki semua jenis pedagang ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_3;
    [SerializeField] private GameObject dateTextGO_3;
    [SerializeField] private GameObject notif_3;
    [SerializeField] private bool notifBool_3;

    private void JuraganPasar() {
        if (PersistentManager.Instance.isSayurPlaced && PersistentManager.Instance.isRempahPlaced && PersistentManager.Instance.isDagingPlaced) {
            if (PersistentManager.Instance.isJuraganPasarPlayed == false) {
                StartCoroutine(PlayNotif_3());
            }
            PersistentManager.Instance.isJuraganPasarPlayed = true;
            notifBool_3 = true;

            buttonCollectPenghargaan_3.SetActive(true);
            buttonCollectPenghargaan_3.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isJuraganPasarClaimed == false) {
                    notifBool_3 = false;
                    buttonCollectPenghargaan_3.SetActive(false);
                    dateTextGO_3.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_3.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(500f);
                    PersistentManager.Instance.isJuraganPasarClaimed = true;

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isJuraganPasarClaimed == true) {
                buttonCollectPenghargaan_3.SetActive(false);
                notifBool_3 = false;
            }
        }
    }

    private IEnumerator PlayNotif_3() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_3.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_3.SetActive(false);
    }

    [Header("--- 4. Memiliki uang senilai 2000k ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_4;
    [SerializeField] private GameObject dateTextGO_4;
    [SerializeField] private GameObject notif_4;
    [SerializeField] private bool notifBool_4;

    private void CuanDiHutan() {
        if (PersistentManager.Instance.dataKoin >= 2000) {
            if (PersistentManager.Instance.isCuanDiHutanPlayed == false) {
                StartCoroutine(PlayNotif_4());
            }
            PersistentManager.Instance.isCuanDiHutanPlayed = true;

            notifBool_4 = true;

            buttonCollectPenghargaan_4.SetActive(true);
            buttonCollectPenghargaan_4.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isCuanDiHutanClaimed == false) {
                    notifBool_4 = false;
                    buttonCollectPenghargaan_4.SetActive(false);
                    dateTextGO_4.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_4.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(200f);
                    PersistentManager.Instance.isCuanDiHutanClaimed = true;

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });
        }
    }
    
    private IEnumerator PlayNotif_4() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_4.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_4.SetActive(false);
    }

    [Header("--- 5. Memasuki pagi hari pertama ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_5;
    [SerializeField] private GameObject dateTextGO_5;
    [SerializeField] private GameObject notif_5;
    [SerializeField] private bool notifBool_5;

    private void HinggaTerbitFajar() {
        if (PersistentManager.Instance.dayCounter == 1) {
            if (PersistentManager.Instance.isHinggaTerbitFajarPlayed == false) {
                StartCoroutine(PlayNotif_5());
            }
            PersistentManager.Instance.isHinggaTerbitFajarPlayed = true;
            notifBool_5 = true;

            buttonCollectPenghargaan_5.SetActive(true);
            buttonCollectPenghargaan_5.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isHinggaTerbitFajarClaimed == false) {
                    notifBool_5 = false;
                    buttonCollectPenghargaan_5.SetActive(false);
                    dateTextGO_5.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_5.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(400f);
                    PersistentManager.Instance.isHinggaTerbitFajarClaimed = true;

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isHinggaTerbitFajarClaimed == true) {
                buttonCollectPenghargaan_5.SetActive(false);
                notifBool_5 = false;
            }
        }
    }

    private IEnumerator PlayNotif_5() {
        yield return new WaitForSeconds(13f);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_5.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_5.SetActive(false);
    }

    [Header("--- 6. Mengalahkan Uto Ijo pertama kali ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_6;
    [SerializeField] private GameObject dateTextGO_6;
    [SerializeField] private GameObject notif_6;
    [SerializeField] private bool notifBool_6;
    
    private void AkuDukun() {
        if (PersistentManager.Instance.isUtoDefeated == true) {
            if (PersistentManager.Instance.isAkuDukunPlayed == false) {
                StartCoroutine(PlayNotif_6());
            }
            PersistentManager.Instance.isAkuDukunPlayed = true;
            notifBool_6 = true;

            buttonCollectPenghargaan_6.SetActive(true);
            buttonCollectPenghargaan_6.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isAkuDukunClaimed == false) {
                    notifBool_6 = false;
                    buttonCollectPenghargaan_6.SetActive(false);
                    dateTextGO_6.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_6.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(500f);
                    PersistentManager.Instance.isAkuDukunClaimed = true;
                    spesialSelectUI.terkunciButtonList[4].gameObject.SetActive(false);

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isAkuDukunClaimed == true) {
                buttonCollectPenghargaan_6.SetActive(false);
                notifBool_6 = false;
            }
        }
    }

    private IEnumerator PlayNotif_6() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_6.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_6.SetActive(false);
    }

    [Header("--- 7. Mengoleksi bahan pertama kali ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_7;
    [SerializeField] private GameObject dateTextGO_7;
    [SerializeField] private GameObject notif_7;
    [SerializeField] private bool notifBool_7;
    
    private void PengepulAlam(string amount, float namaCollectable) {
        if (PersistentManager.Instance.isBatuCollected || PersistentManager.Instance.isKayuCollected || PersistentManager.Instance.isTanahLiatCollected || PersistentManager.Instance.isAkikCollected) {
            if (PersistentManager.Instance.isPengepulAlamPlayed == false) {
                StartCoroutine(PlayNotif_7());
            }
            PersistentManager.Instance.isPengepulAlamPlayed = true;
            notifBool_7 = true;

            buttonCollectPenghargaan_7.SetActive(true);
            buttonCollectPenghargaan_7.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isPengepulAlamClaimed == false) {
                    notifBool_7 = false;
                    buttonCollectPenghargaan_7.SetActive(false);
                    dateTextGO_7.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_7.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(200f);
                    PersistentManager.Instance.isPengepulAlamClaimed = true;

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isPengepulAlamClaimed == true) {
                buttonCollectPenghargaan_7.SetActive(false);
                notifBool_7 = false;
            }
        }
    }

    private IEnumerator PlayNotif_7() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_7.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_7.SetActive(false);
    }

    [Header("--- 8. Mengoleksi semua bahan ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_8;
    [SerializeField] private GameObject dateTextGO_8;
    [SerializeField] private GameObject notif_8;
    [SerializeField] private bool notifBool_8;
    
    private void KuliSakti(string amount, float namaCollectable) {
        if (PersistentManager.Instance.isBatuCollected && PersistentManager.Instance.isKayuCollected && PersistentManager.Instance.isTanahLiatCollected && PersistentManager.Instance.isAkikCollected) {
            if (PersistentManager.Instance.isKuliSaktiPlayed == false) {
                StartCoroutine(PlayNotif_8());
            }
            PersistentManager.Instance.isKuliSaktiPlayed = true;
            notifBool_8 = true;

            buttonCollectPenghargaan_8.SetActive(true);
            buttonCollectPenghargaan_8.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isKuliSaktiClaimed == false) {
                    notifBool_8 = false;
                    buttonCollectPenghargaan_8.SetActive(false);
                    dateTextGO_8.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_8.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(300f);
                    PersistentManager.Instance.isKuliSaktiClaimed = true;

                    spesialSelectUI.terkunciButtonList[3].gameObject.SetActive(false);

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isKuliSaktiClaimed == true) {
                buttonCollectPenghargaan_8.SetActive(false);
                notifBool_8 = false;
            }
        }
    }

    private IEnumerator PlayNotif_8() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_8.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_8.SetActive(false);
    }

    [Header("--- 9. Memasuki malam kedua ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_9;
    [SerializeField] private GameObject dateTextGO_9;
    [SerializeField] private GameObject notif_9;
    [SerializeField] private bool notifBool_9;
    
    private void MalamKliwon() {
        if (PersistentManager.Instance.nightCounter == 2) {
            if (PersistentManager.Instance.isMalamKliwonPlayed == false) {
                StartCoroutine(PlayNotif_9());
            }
            PersistentManager.Instance.isMalamKliwonPlayed = true;
            notifBool_9 = true;

            buttonCollectPenghargaan_9.SetActive(true);
            buttonCollectPenghargaan_9.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isMalamKliwonClaimed == false) {
                    notifBool_9 = false;
                    buttonCollectPenghargaan_9.SetActive(false);
                    dateTextGO_9.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_9.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(400f);
                    PersistentManager.Instance.isMalamKliwonClaimed = true;

                    spesialSelectUI.terkunciButtonList[2].gameObject.SetActive(false);

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });

            if (PersistentManager.Instance.isMalamKliwonClaimed == true) {
                buttonCollectPenghargaan_9.SetActive(false);
                notifBool_9 = false;
            }
        }
    }

    private IEnumerator PlayNotif_9() {
        yield return new WaitForSeconds(1f);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_9.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_9.SetActive(false);
    }

    [Header("--- 10. Memiliki 4000K ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_10;
    [SerializeField] private GameObject dateTextGO_10;
    [SerializeField] private GameObject notif_10;
    [SerializeField] private bool notifBool_10;

    private void PebisnisGhaib() {
        if (PersistentManager.Instance.dataKoin >= 5000) {
            if (PersistentManager.Instance.isPebisnisGhaibPlayed == false) {
                StartCoroutine(PlayNotif_10());
            }
            PersistentManager.Instance.isPebisnisGhaibPlayed = true;

            notifBool_10 = true;

            buttonCollectPenghargaan_10.SetActive(true);
            buttonCollectPenghargaan_10.GetComponent<Button>().onClick.AddListener(() => {
                if (PersistentManager.Instance.isPebisnisGhaibClaimed == false) {
                    notifBool_10 = false;
                    buttonCollectPenghargaan_10.SetActive(false);
                    dateTextGO_10.SetActive(true);
                    TextMeshProUGUI dateText = dateTextGO_10.GetComponent<TextMeshProUGUI>();
                    dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PersistentManager.Instance.UpdateKoin(500f);
                    PersistentManager.Instance.isPebisnisGhaibClaimed = true;

                    spesialSelectUI.terkunciButtonList[5].gameObject.SetActive(false);

                    StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
                }
            });
        }
        
    }

    private IEnumerator PlayNotif_10() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.achievmentNotif);
        notif_10.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_10.SetActive(false);
    }
}