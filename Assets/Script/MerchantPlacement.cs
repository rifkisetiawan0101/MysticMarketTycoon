using UnityEngine;
using UnityEngine.UI;

public class MerchantPlacement : MonoBehaviour {
    public static MerchantPlacement Instance { get; private set; }
    private MerchantManager merchantManager;
    private Vector3 placementPosition;

    private void Start() {
        MerchantManager.Instance.rotateIndex = 0;
    }

    public void Setup(Vector3 position, MerchantManager manager) {
        placementPosition = position;
        merchantManager = manager;

        // Menemukan tombol dan menambahkan listener
        Button buttonAccept = transform.Find("Canvas/ButtonAccept").GetComponent<Button>();
        Button buttonCancel = transform.Find("Canvas/ButtonCancel").GetComponent<Button>();
        Button buttonRotate = transform.Find("Canvas/ButtonRotate").GetComponent<Button>();

        buttonAccept.onClick.AddListener(() => AcceptButtonPlacement());
        buttonCancel.onClick.AddListener(() => CancelButtonPlacement());
        buttonRotate.onClick.AddListener(() => RotateButtonPlacement());

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            merchantManager.CancelPlacement();
            Destroy(gameObject);
        });

        Button buttonSpesial = GameObject.Find("ButtonSpesial").GetComponent<Button>(); 
        buttonSpesial.onClick.AddListener(() => {
            merchantManager.CancelPlacement();
            Destroy(gameObject);
        });
    }

    private void RotateButtonPlacement() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        
        MerchantManager.Instance.rotateIndex++;
        if (MerchantManager.Instance.rotateIndex > 2) {
            MerchantManager.Instance.rotateIndex = 0;
        }

        UpdateGFX();
    }

    [SerializeField] private Sprite GFXSayurKanan;
    [SerializeField] private Sprite GFXSayurKiri;
    [SerializeField] private Sprite GFXSayurBawah;

    [SerializeField] private Sprite GFXRempahKanan;
    [SerializeField] private Sprite GFXRempahKiri;
    [SerializeField] private Sprite GFXRempahBawah;

    [SerializeField] private Sprite GFXDagingKanan;
    [SerializeField] private Sprite GFXDagingKiri;
    [SerializeField] private Sprite GFXDagingBawah;
    [SerializeField] private GameObject merchantGFX;
    
    private void UpdateGFX() {
        // Dapatkan tipe merchant aktif
        string merchantTypeName = MerchantManager.Instance.GetActiveMerchantTypeName();
    
        // Periksa tipe merchant
        switch (merchantTypeName) {
            case "Pedagang Sayur":
                // Cek rotateIndex untuk Pedagang Sayur
                switch (MerchantManager.Instance.rotateIndex) {
                    case 0: // Kanan
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXSayurKanan;
                        break;
                    case 1: // Kiri
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXSayurKiri;
                        break;
                    case 2: // Bawah
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXSayurBawah;
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
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXRempahKiri;
                        break;
                    case 1: // Bawah
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXRempahBawah;
                        break;
                    case 2: // Kanan
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXRempahKanan;
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
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXDagingBawah;
                        break;
                    case 1: // Kanan
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXDagingKanan;
                        break;
                    case 2: // Kiri
                        merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXDagingKiri;
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

    private void AcceptButtonPlacement() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        // Panggil MerchantPlacing di MerchantManager
        merchantManager.MerchantPlacing(placementPosition);
        Destroy(gameObject); // Menghancurkan prefab placement setelah diterima
        StopCoroutine(FeedbackManager.instance.PlayComingSoonNotif());
        FeedbackManager.instance.comingSoonNotif.SetActive(false);
    }

    private void CancelButtonPlacement() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        merchantManager.CancelPlacement(); // Reset status placement di MerchantManager
        Destroy(gameObject); // Menghancurkan prefab placement jika dibatalkan
        StopCoroutine(FeedbackManager.instance.PlayComingSoonNotif());
        FeedbackManager.instance.comingSoonNotif.SetActive(false);
    } 
}
