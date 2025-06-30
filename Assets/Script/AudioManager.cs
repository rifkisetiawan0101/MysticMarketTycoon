using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public static AudioManager audioManager;

    [Header ("---Audio SOurce---")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("---Non Button Sound---")]
    public AudioClip introBacksound;
    public AudioClip inGameBacksound;
    public AudioClip inGamePagiBacksound;
    public AudioClip creditBacksound;
    public AudioClip sceneTransitionSound;
    public AudioClip utoNotif;
    public AudioClip battleBacksound;
    public AudioClip placeAndUpgradeBuilding;
    public AudioClip attackButtonSound;
    public AudioClip winFlashSound;
    public AudioClip walk;
    public AudioClip invoiceSound;
    public AudioClip achievmentNotif;
    public AudioClip coinMinus;
    public AudioClip clickJalan;
    public AudioClip lessKoinSound;
    public AudioClip placementSound;
    public AudioClip countDownSound;
    public AudioClip winMiniGameSound;
    public AudioClip loseMiniGameSound;
    public AudioClip dragStartSound;
    public AudioClip dragEndSound;
    public AudioClip loseFlashSound;

    [Header ("---Button Sound---")]
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    public AudioClip achievmentCollect;
    public AudioClip collectAkik;
    public AudioClip collectCoin;
    public AudioClip pagiCollectItem;
    public AudioClip buyItem;
    public AudioClip miniGameClick;

    public bool hasInvoiceSoundPlayed = false; // Tambahkan flag ini

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "InGame" || SceneManager.GetActiveScene().name != "InGamePagi")
        {
            StartCoroutine(PlayIntroBacksound(3.5f)); // Memainkan introBacksound setelah 3.5 detik di scene pertama
        }
    }

    private void Update()
    {
        // string currentSceneName = SceneManager.GetActiveScene().name;

        // if (PersistentManager.Instance.isInvoiceShown == true && !hasInvoiceSoundPlayed)
        // {
        //     // Pastikan tidak menjalankan jika berada di scene yang tidak diinginkan
        //     if (currentSceneName != "InGamePagiFirstCutScene" && currentSceneName != "InGamePagiCutScene")
        //     {
        //         musicSource.Stop();
        //         PlaySFX(invoiceSound);
        //         hasInvoiceSoundPlayed = true; // Set flag menjadi true agar tidak dipanggil lagi
        //     }
        // }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hasInvoiceSoundPlayed = false;

        if (scene.name == "HomeScreen")
        {
            musicSource.clip = introBacksound;
            musicSource.Play();
        }

        if (scene.name == "InGame")
        {
            musicSource.clip = inGameBacksound;
            musicSource.Play();
        }

        if (scene.name == "InGamePagi")
        {
            musicSource.clip = inGamePagiBacksound;
            musicSource.Play();
        }
    }



    private IEnumerator PlayIntroBacksound(float waitTime) { 
        yield return new WaitForSeconds(waitTime);

        musicSource.clip = introBacksound;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // gnti music
    public void ChangeMusic(AudioClip newClip, float fadeDuration)
    {
        if (musicSource.clip != newClip)
        {
            StartCoroutine(FadeOutAndChangeMusicCoroutine(newClip, fadeDuration));
        }
    }

    // fadeo Out
    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset volume ke nilai awal
    }

    //
    private IEnumerator FadeOutAndChangeMusicCoroutine(AudioClip newClip, float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop(); // Hentikan musik setelah fade out
        musicSource.volume = startVolume; // Reset volume ke nilai awal
        musicSource.clip = newClip; // Ganti musik ke yang baru
        musicSource.Play(); // Putar musik baru
    }
}


