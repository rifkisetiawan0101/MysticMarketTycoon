using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FeedbackManager : MonoBehaviour
{   
    public static FeedbackManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private float delay_18 = 0.055f; // 18fps
    private float delay_9 = 0.111f; // 9fps
    [Header("--- Feedback Click Drop Item ---")]
    [SerializeField] private GameObject collectFeedback;
    [SerializeField] private Image collectFeedbackImage;
    [SerializeField] private Sprite[] collectFrames;

    [Header("--- Feedback Collect Coin ---")]
    [SerializeField] private GameObject collectCoinFeedback;
    [SerializeField] private Image collectCoinFeedbackImage;
    [SerializeField] private Sprite[] collectCoinFrames;

    [Header("--- Feedback Rojak Kalah ---")]
    [SerializeField] private GameObject rojakKalahFeedback;
    [SerializeField] private Image rojakKalahFeedbackImage;
    [SerializeField] private Sprite[] rojakKalahFrames;

    public IEnumerator PlayCollectBatuAkik()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.collectAkik);
        collectFeedback.SetActive(true);
        for (int i = 0; i < collectFrames.Length; i++)
        {
            collectFeedbackImage.sprite = collectFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        collectFeedback.SetActive(false);
    }

    public IEnumerator PlayCollectKoin()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.collectCoin);
        collectCoinFeedback.SetActive(true);
        for (int i = 0; i < collectFrames.Length; i++)
        {
            collectCoinFeedbackImage.sprite = collectCoinFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        collectCoinFeedback.SetActive(false);
    }

    public IEnumerator RojakKalahFeedback()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.coinMinus);
        rojakKalahFeedback.SetActive(true);
        for (int i = 0; i < rojakKalahFrames.Length; i++)
        {
            rojakKalahFeedbackImage.sprite = rojakKalahFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        rojakKalahFeedback.SetActive(false);
    }

    [Header("--- LessKoin Notif ---")]
    [SerializeField] private GameObject lessKoinNotif;
    [SerializeField] private Image lessKoinNotifImage;
    [SerializeField] private Sprite[] lessKoinNotifFrames;
    public IEnumerator PlayLessKoinNotif() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.lessKoinSound);
        lessKoinNotif.SetActive(true);
        for (int i = 0; i < lessKoinNotifFrames.Length; i++) {
            lessKoinNotifImage.sprite = lessKoinNotifFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        lessKoinNotif.SetActive(false);
    }

    [Header("--- LahanBuruk Notif ---")]
    [SerializeField] private GameObject lahanBurukNotif;
    [SerializeField] private Image lahanBurukNotifImage;
    [SerializeField] private Sprite[] lahanBurukNotifFrames;

    public IEnumerator PlayLahanBurukNotif() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.coinMinus);
        lahanBurukNotif.SetActive(true);
        for (int i = 0; i < lahanBurukNotifFrames.Length; i++) {
            lahanBurukNotifImage.sprite = lahanBurukNotifFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        lahanBurukNotif.SetActive(false);
    }

    [Header("--- ComingSoon Notif ---")]
    [SerializeField] public GameObject comingSoonNotif;
    [SerializeField] private Image comingSoonNotifImage;
    [SerializeField] private Sprite[] comingSoonNotifFrames;

    public IEnumerator PlayComingSoonNotif() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.lessKoinSound);
        comingSoonNotif.SetActive(true);
        for (int i = 0; i < comingSoonNotifFrames.Length; i++) {
            comingSoonNotifImage.sprite = comingSoonNotifFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        comingSoonNotif.SetActive(false);
    }
    
    [Header("--- Notif Uto dan Malam Timer ---")]
    [SerializeField] private GameObject notifUto;
    [SerializeField] private GameObject notifMalam;
    public IEnumerator PlayNotifMalam() {
        notifMalam.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notifMalam.SetActive(false);
    }

    public IEnumerator PlayNotifUto() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.utoNotif);
        notifUto.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notifUto.SetActive(false);
    }

    [Header("--- LessKoin Upgrade ---")]
    [SerializeField] private GameObject lessKoinUpgrade;
    [SerializeField] private Image lessKoinUpgradeImage;
    [SerializeField] private Sprite[] lessKoinUpgradeFrames;
    public IEnumerator PlayLessKoinUpgrade() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.lessKoinSound);
        lessKoinUpgrade.SetActive(true);
        for (int i = 0; i < lessKoinUpgradeFrames.Length; i++) {
            lessKoinUpgradeImage.sprite = lessKoinUpgradeFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        lessKoinUpgrade.SetActive(false);
    }

    [Header("--- Batu Collect ---")]
    [SerializeField] private GameObject batuCollect;
    [SerializeField] private Image batuCollectImage;
    [SerializeField] private Sprite[] batuCollectFrames;
    public IEnumerator PlayBatuCollect(GameObject item) {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.pagiCollectItem);
        GameObject prefab = Instantiate(batuCollect, transform.position, Quaternion.identity);
        batuCollectImage = prefab.GetComponentInChildren<Image>();
        for (int i = 0; i < batuCollectFrames.Length; i++) {
            batuCollectImage.sprite = batuCollectFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(prefab);
        Destroy(item);
    }

    [Header("--- Kayu Collect ---")]
    [SerializeField] private GameObject kayuCollect;
    [SerializeField] private Image kayuCollectImage;
    [SerializeField] private Sprite[] kayuCollectFrames;
    public IEnumerator PlayKayuCollect(GameObject item) {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.pagiCollectItem);
        GameObject prefab = Instantiate(kayuCollect, transform.position, Quaternion.identity);
        kayuCollectImage = prefab.GetComponentInChildren<Image>();
        for (int i = 0; i < kayuCollectFrames.Length; i++) {
            kayuCollectImage.sprite = kayuCollectFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(prefab);
        Destroy(item);
    }

    [Header("--- Tanah Collect ---")]
    [SerializeField] private GameObject tanahCollect;
    [SerializeField] private Image tanahCollectImage;
    [SerializeField] private Sprite[] tanahCollectFrames;
    public IEnumerator PlayTanahCollect(GameObject item) {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.pagiCollectItem);
        GameObject prefab = Instantiate(tanahCollect, transform.position, Quaternion.identity);
        tanahCollectImage = prefab.GetComponentInChildren<Image>();
        for (int i = 0; i < tanahCollectFrames.Length; i++) {
            tanahCollectImage.sprite = tanahCollectFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(prefab);
        Destroy(item);
    }

    [Header("--- Akik Collect ---")]
    [SerializeField] private GameObject akikCollect;
    [SerializeField] private Image akikCollectImage;
    [SerializeField] private Sprite[] akikCollectFrames;
    public IEnumerator PlayAkikCollect(GameObject item) {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.pagiCollectItem);
        GameObject prefab = Instantiate(akikCollect, transform.position, Quaternion.identity);
        akikCollectImage = prefab.GetComponentInChildren<Image>();
        for (int i = 0; i < akikCollectFrames.Length; i++) {
            akikCollectImage.sprite = akikCollectFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(prefab);
        Destroy(item);
    }

    [Header("--- Beli Sayur ---")]
    [SerializeField] private GameObject beliSayurPrefab;
    private Image beliSayurImage;
    [SerializeField] private Sprite[] beliSayurFrames;

    public IEnumerator PlayBeliSayur() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buyItem);//doble di restok ui juga
        GameObject beliSayur = Instantiate(beliSayurPrefab, transform.position, Quaternion.identity);
        beliSayurImage = beliSayur.GetComponentInChildren<Image>();
        for (int i = 0; i < beliSayurFrames.Length; i++) {
            beliSayurImage.sprite = beliSayurFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(beliSayur);
    }

    [Header("--- Beli Rempah ---")]
    [SerializeField] private GameObject beliRempahPrefab;
    private Image beliRempahImage;
    [SerializeField] private Sprite[] beliRempahFrames;

    public IEnumerator PlayBeliRempah() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buyItem);//doble di restok ui juga
        GameObject beliRempah = Instantiate(beliRempahPrefab, transform.position, Quaternion.identity);
        beliRempahImage = beliRempah.GetComponentInChildren<Image>();
        for (int i = 0; i < beliRempahFrames.Length; i++) {
            beliRempahImage.sprite = beliRempahFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(beliRempah);
    }

    [Header("--- Beli Daging ---")]
    [SerializeField] private GameObject beliDagingPrefab;
    private Image beliDagingImage;
    [SerializeField] private Sprite[] beliDagingFrames;

    public IEnumerator PlayBeliDaging() {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buyItem);//doble di restok ui juga
        GameObject beliDaging = Instantiate(beliDagingPrefab, transform.position, Quaternion.identity);
        beliDagingImage = beliDaging.GetComponentInChildren<Image>();
        for (int i = 0; i < beliDagingFrames.Length; i++) {
            beliDagingImage.sprite = beliDagingFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        Destroy(beliDaging);
    }

    [Header("--- Asep Furnitur dan Hiasan ---")]
    [SerializeField] private Sprite[] furniturHiasanFrames;
    private Image furniturHiasanImage;

    public IEnumerator PlayFurniturHiasan(GameObject furniturHiasan) {
        furniturHiasanImage = furniturHiasan.GetComponentInChildren<Image>();
        for (int i = 0; i < furniturHiasanFrames.Length; i++) {
            furniturHiasanImage.sprite = furniturHiasanFrames[i];
            yield return new WaitForSeconds(delay_9);
        }
    }

    [Header("--- Sihir ---")]
    [SerializeField] private GameObject sihirFeedback;
    [SerializeField] private Sprite[] sihirFeedbackFrames;
    public IEnumerator PlaySihirFeedback() {
        sihirFeedback.SetActive(true);
        for (int i = 0; i < sihirFeedbackFrames.Length; i++) {
            sihirFeedback.GetComponent<Image>().sprite = sihirFeedbackFrames[i];
            yield return new WaitForSeconds(delay_18);
        }
        sihirFeedback.SetActive(false);
    }

    private float delay_12 = 0.083f;

    [Header("--- Boss Battle ---")]
    [SerializeField] private GameObject bossBattleFeedback;
    [SerializeField] private Sprite[] bossBattleFeedbackFrames;
    public IEnumerator PlayBossBattleFeedback() {
        bossBattleFeedback.SetActive(true);
        while (PersistentManager.Instance.isBattleBoss == true) {
            for (int i = 0; i < bossBattleFeedbackFrames.Length; i++) {
                // Cek ulang apakah isBattleBoss masih true di setiap iterasi
                if (PersistentManager.Instance.isBattleBoss == false) {
                    bossBattleFeedback.SetActive(false);
                    bossBattleFeedback.GetComponent<Image>().sprite = bossBattleFeedbackFrames[0];
                    yield break; // Keluar dari coroutine
                }
                bossBattleFeedback.GetComponent<Image>().sprite = bossBattleFeedbackFrames[i];
                yield return new WaitForSeconds(delay_12);
            }
        }
        // Set ulang tampilan jika keluar dari loop
        bossBattleFeedback.SetActive(false);
        bossBattleFeedback.GetComponent<Image>().sprite = bossBattleFeedbackFrames[0];
    }


    [Header("--- Uto Battle ---")]
    [SerializeField] private GameObject utoBattleFeedback;
    [SerializeField] private Sprite[] utoBattleFeedbackFrames;
    public IEnumerator PlayUtoBattleFeedback() {
        utoBattleFeedback.SetActive(true);
        while (PersistentManager.Instance.isBattleUto == true) {
            for (int i = 0; i < utoBattleFeedbackFrames.Length; i++) {
                // Cek ulang apakah isBattleUto masih true di setiap iterasi
                if (PersistentManager.Instance.isBattleUto == false) {
                    utoBattleFeedback.SetActive(false);
                    utoBattleFeedback.GetComponent<Image>().sprite = utoBattleFeedbackFrames[0];
                    yield break; // Keluar dari coroutine
                }
                utoBattleFeedback.GetComponent<Image>().sprite = utoBattleFeedbackFrames[i];
                yield return new WaitForSeconds(delay_12);
            }
        }
        // Set ulang tampilan jika keluar dari loop
        utoBattleFeedback.SetActive(false);
        utoBattleFeedback.GetComponent<Image>().sprite = utoBattleFeedbackFrames[0];
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        sihirFeedback = GameObject.Find("Player/CanvasSihirWorld/SihirFeedback");
    }
}
