using UnityEngine;
using System.Collections;

public class MerchantConstruction : MonoBehaviour{
    [SerializeField] private float timeToConstruct = 2f;
    private float contructionTimer;
    // private Animator animator;

    // private void Start() {
    //     animator = GetComponent<Animator>();
    // }

    private void Start() {
        merchantIndex = MerchantManager.Instance.GetCurrentMerchantIndex();
        rotateIndex = MerchantManager.Instance.GetCurrentRotateIndex();
    }

    private int merchantIndex;
    private int rotateIndex;
    private bool isPlayingGFX;
    private void Update() {
        if (!isPlayingGFX) {
            UpdateGFX();
        }
        contructionTimer += Time.deltaTime;
        
        if (contructionTimer >= timeToConstruct) {
            UpdateInstantiate();
            Destroy(gameObject);
        }
    }
    
    private void UpdateGFX() {
        // Dapatkan tipe merchant aktif
        string merchantTypeName = MerchantManager.Instance.GetActiveMerchantTypeName();
    
        // Periksa tipe merchant
        switch (merchantTypeName) {
            case "Pedagang Sayur":
                // Cek rotateIndex untuk Pedagang Sayur
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Kanan
                        StartCoroutine(PlayGFXSayurKanan());
                        break;
                    case 1: // Kiri
                        StartCoroutine(PlayGFXSayurKiri());
                        break;
                    case 2: // Bawah
                        StartCoroutine(PlayGFXSayurTengah());
                        break;
                    default:
                        Debug.LogWarning("Rotate Index tidak valid untuk Pedagang Sayur!");
                        break;
                }
                break;

            case "Pemasok Rempah":
                // Cek rotateIndex untuk Pemasok Rempah
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Kiri
                        StartCoroutine(PlayGFXRempahKiri());
                        break;
                    case 1: // Bawah
                        StartCoroutine(PlayGFXRempahTengah());
                        break;
                    case 2: // Kanan
                        StartCoroutine(PlayGFXRempahKanan());
                        break;
                    default:
                        Debug.LogWarning("Rotate Index tidak valid untuk Pemasok Rempah!");
                        break;
                }
                break;

            case "Penjual Daging":
                // Cek rotateIndex untuk Penjual Daging
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Bawah
                        StartCoroutine(PlayGFXDagingTengah());
                        break;
                    case 1: // Kanan
                        StartCoroutine(PlayGFXDagingKanan());
                        break;
                    case 2: // Kiri
                        StartCoroutine(PlayGFXDagingKiri());
                        break;
                    default:
                        Debug.LogWarning("Rotate Index tidak valid untuk Penjual Daging!");
                        break;
                }
                break;

            default:
                Debug.LogWarning("Tipe Merchant tidak dikenali: " + merchantTypeName);
                break;
        }
    }

    private float delay_12 = 1f / 12f;
    [Header("--- Animation ---")]
    [SerializeField] private Sprite[] sayurKiriFrames;
    public IEnumerator PlayGFXSayurKiri() {
        isPlayingGFX = true;
        for (int i = 0; i < sayurKiriFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = sayurKiriFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] sayurKananFrames;
    public IEnumerator PlayGFXSayurKanan() {
        isPlayingGFX = true;
        for (int i = 0; i < sayurKananFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = sayurKananFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] sayurTengahFrames;
    public IEnumerator PlayGFXSayurTengah() {
        isPlayingGFX = true;
        for (int i = 0; i < sayurTengahFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = sayurTengahFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] rempahKiriFrames;
    public IEnumerator PlayGFXRempahKiri() {
        isPlayingGFX = true;
        for (int i = 0; i < rempahKiriFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = rempahKiriFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] rempahKananFrames;
    public IEnumerator PlayGFXRempahKanan() {
        isPlayingGFX = true;
        for (int i = 0; i < rempahKananFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = rempahKananFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] rempahTengahFrames;
    public IEnumerator PlayGFXRempahTengah() {
        isPlayingGFX = true;
        for (int i = 0; i < rempahTengahFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = rempahTengahFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] dagingKiriFrames;
    public IEnumerator PlayGFXDagingKiri() {
        isPlayingGFX = true;
        for (int i = 0; i < dagingKiriFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = dagingKiriFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] dagingKananFrames;
    public IEnumerator PlayGFXDagingKanan() {
        isPlayingGFX = true;
        for (int i = 0; i < dagingKananFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = dagingKananFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    [SerializeField] private Sprite[] dagingTengahFrames;
    public IEnumerator PlayGFXDagingTengah() {
        isPlayingGFX = true;
        for (int i = 0; i < dagingTengahFrames.Length; i++) {
            gameObject.GetComponent<SpriteRenderer>().sprite = dagingTengahFrames[i];
            yield return new WaitForSeconds(delay_12);
        }
    }

    private void UpdateInstantiate() {
        // Dapatkan tipe merchant aktif
        string merchantTypeName = MerchantManager.Instance.GetActiveMerchantTypeName();
    
        // Periksa tipe merchant
        switch (merchantTypeName) {
            case "Pedagang Sayur":
                // Cek rotateIndex untuk Pedagang Sayur
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Kanan
                        instantiatedObject = Instantiate(targetPrefabSayurKanan.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    case 1: // Kiri
                        instantiatedObject = Instantiate(targetPrefabSayurKiri.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    case 2: // Bawah
                        instantiatedObject = Instantiate(targetPrefabSayurBawah.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    default:
                        Debug.LogWarning("Rotate Index tidak valid untuk Pedagang Sayur!");
                        break;
                }
                break;

            case "Pemasok Rempah":
                // Cek rotateIndex untuk Pemasok Rempah
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Kiri
                        instantiatedObject = Instantiate(targetPrefabRempahKiri.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    case 1: // Bawah
                        instantiatedObject = Instantiate(targetPrefabRempahBawah.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    case 2: // Kanan
                        instantiatedObject = Instantiate(targetPrefabRempahKanan.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    default:
                        Debug.LogWarning("Rotate Index tidak valid untuk Pemasok Rempah!");
                        break;
                }
                break;

            case "Penjual Daging":
                // Cek rotateIndex untuk Penjual Daging
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Bawah
                        instantiatedObject = Instantiate(targetPrefabDagingBawah.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    case 1: // Kanan
                        instantiatedObject = Instantiate(targetPrefabDagingKanan.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    case 2: // Kiri
                        instantiatedObject = Instantiate(targetPrefabDagingKiri.gameObject, transform.position, Quaternion.identity);
                        merchant = instantiatedObject.GetComponent<IMerchant>();
                        merchant.InitData(merchantIndex, rotateIndex);
                        break;
                    default:
                        Debug.LogWarning("Rotate Index tidak valid untuk Penjual Daging!");
                        break;
                }
                break;

            default:
                Debug.LogWarning("Tipe Merchant tidak dikenali: " + merchantTypeName);
                break;
        }
    }
    
    [SerializeField] private Transform targetPrefabSayurKanan;
    [SerializeField] private Transform targetPrefabSayurKiri;
    [SerializeField] private Transform targetPrefabSayurBawah;
    [SerializeField] private Transform targetPrefabRempahKanan;
    [SerializeField] private Transform targetPrefabRempahKiri;
    [SerializeField] private Transform targetPrefabRempahBawah;
    [SerializeField] private Transform targetPrefabDagingKanan;
    [SerializeField] private Transform targetPrefabDagingKiri;
    [SerializeField] private Transform targetPrefabDagingBawah;
    private GameObject instantiatedObject;
    private IMerchant merchant;
    
}
