using Pathfinding;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PremanButoAI : MonoBehaviour
{
    public static PremanButoAI Instance;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name != "InGame") {
            Destroy(gameObject);
        }
    }

    private Seeker seeker;

    [SerializeField] private GameObject premanButoGFX; // Menyimpan referensi ke GFX
    [SerializeField] private GameObject winFlash;
    // [SerializeField] private TextMeshProUGUI premanButoHealthUI;
    [SerializeField] private Image blueBarImage;

    public GameObject overlay;
    public GameObject collectDropItemButton; // Tambahkan referensi ke CollectDropItemButton
    public GameObject premanButoDropItem;
    public float buttoHealth = 60f; // Health awal 30
    public float maxHealth = 100f; // Health maksimal

    public float speed = 300;
    public float smoothTime = 0.1f;
    public float pauseTime = 3f; // Waktu berhenti di target
    public bool isPremanArrived = false;
    public bool isBattle = false;
    public bool isUtoMove = false;

    private MerchantManager merchantManager;
    private Vector3 targetPosition;
    private Vector3 spawnPosition; // Posisi awal spawn NPC
    private PremanButoAttackTrigger premanButoAttackTrigger;

    Rigidbody2D rb;
    Animator animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private Vector2 velocity;
    private Vector2 _oldPosition;
    private Coroutine increaseHealthCoroutine; // Coroutine untuk menambah health


    public void SetupNPC(MerchantManager manager)
    {
        // Inisialisasi MerchantManager dan posisi spawn
        merchantManager = manager;
        spawnPosition = transform.position;
        SetRandomTarget();
    }

    private void Start()
    {   
        player = GameObject.Find("Player");
        movePosisiKalah = GameObject.Find("MovePosisiKalah");

        premanButoAttackTrigger = GetComponentInChildren<PremanButoAttackTrigger>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        seeker = GetComponent<Seeker>();
        _oldPosition = rb.position;

        if (merchantManager != null)
        {
            SetRandomTarget();
            StartCoroutine(ButoLoop());
        }
        else
        {
            Debug.LogWarning("MerchantManager belum di-set.");
        }

        premanButoDropItem.gameObject.SetActive(false); // Nonaktifkan DropItem di awal
        UpdateHealthUI(); // Update UI saat starts

        if (SceneManager.GetActiveScene().name != "InGame") {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        NpcAnimation();
    }

    private void NpcAnimation() {
        Vector2 newPosition = transform.position;
        Vector2 movement = (newPosition - _oldPosition).normalized;
        _oldPosition = newPosition;

        if (movement != Vector2.zero) {
            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);
            animator.SetFloat(_lastHorizontal, movement.x);
            animator.SetFloat(_lastVertical, movement.y);

            if (isUtoMove) {
                animator.Play("Move");
            } else {
                animator.Play("Idle");
            }
        }
    }

    private void SetRandomTarget()
    {
        if (PersistentManager.Instance.dataMerchantList.Count > 0)
        {
            int randomIndex = Random.Range(0, PersistentManager.Instance.dataMerchantList.Count);
            targetPosition = PersistentManager.Instance.dataMerchantList[randomIndex].merchantPosition;
        }
    }

    private IEnumerator MoveToTarget()
    {
        isUtoMove = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed);
            rb.MovePosition(targetPos);
            _oldPosition = rb.position;

            yield return null;
        }

        isPremanArrived = true; // Menandakan bahwa Buto sudah sampai di lokasi target
        isUtoMove = false;
        // HandleArrivalAtTarget(); // Memanggil fungsi untuk menangani setelah Buto sampai di target
        StartCoroutine(PauseAtTargetAndCollectCoins());
    }

    // private void ReturnToSpawn()
    // {
    //     StartCoroutine(MoveToSpawn());
    // }

    // private IEnumerator MoveToSpawn()
    // {
    //     while (Vector3.Distance(transform.position, spawnPosition) > 0.1f)
    //     {
    //         Vector2 targetPos = Vector2.SmoothDamp(rb.position, spawnPosition, ref velocity, smoothTime, speed);
    //         rb.MovePosition(targetPos);
    //         yield return null;
    //     }

    //     Destroy(gameObject); // Menghancurkan NPC setelah kembali ke posisi awal
    // }

    // ------------------------------------- BARU ------------------------------------------------------------ //

    private GameObject player;
    private GameObject movePosisiKalah;

    public void UpdateButoHealth(float amount)
    {
        // Update health dan pastikan tidak kurang dari 0 atau lebih dari maxHealth
        buttoHealth = Mathf.Clamp(buttoHealth + amount, 0, maxHealth);

        Debug.Log("Update Health UI: " + buttoHealth);

        UpdateHealthUI();

        if (buttoHealth <= 0)
        {
            HandleButoDefeated();
        }
        else if (buttoHealth >= maxHealth) // preman buto menang
        {
            StartCoroutine(PlayLoseFlash());
            isBattle = false;
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);
            StartCoroutine(FeedbackManager.instance.RojakKalahFeedback());

            if (PersistentManager.Instance.dataKoin >= 100) {
                PersistentManager.Instance.UpdateKoin(-100);
            } else if (PersistentManager.Instance.dataKoin < 100) {
                PersistentManager.Instance.dataKoin = 0;
            }

            player.transform.position = movePosisiKalah.transform.position;
            
            ResetHealthAndStop();
            buttoHealth = 60; // Reset darah ke 60
            overlay.SetActive(false);
            premanButoAttackTrigger.serangButoButton.SetActive(false); 
            StartCoroutine(ButoLoop());

            PersistentManager.Instance.isBattleUto = false;
            StopCoroutine(FeedbackManager.instance.PlayUtoBattleFeedback());
        }
    }

    private void UpdateHealthUI()
    {
        // premanButoHealthUI.text = "HP: " + buttoHealth.ToString("F0");
        blueBarImage.fillAmount = buttoHealth / maxHealth;
    }

    // private void HandleArrivalAtTarget()
    // {
    //     if (premanButoAttackTrigger.isPlayerEnterTrigger)
    //     {
    //         premanButoAttackTrigger.ActivateSerangButton(); // Aktifkan tombol serang
    //         // StartIncreasingHealth(); // Mulai penambahan health jika pemain berada di dalam trigger
    //     }
    // }

    private void HandleButoDefeated() {   
        premanButoGFX.SetActive(false);
        premanButoDropItem.SetActive(true);
        premanButoAttackTrigger.serangButoButton.SetActive(false);
        // premanButoHealthUI.text = " ";
        buttoHealth = 0;
        premanButoAttackTrigger.isPlayerEnterTrigger = false;
        premanButoAttackTrigger.GetComponent<CircleCollider2D>().enabled = false;
        overlay.SetActive(false);
        isBattle = false;
        StartCoroutine(PlayWInFlash());
        speed = 0;
        StopCoroutine(MoveToTarget());
        StopCoroutine(ButoLoop());
        StopCoroutine(IncreaseHealthOverTime());
        StartCoroutine(CheckAndActivateCollectButton());

        PersistentManager.Instance.isBattleUto = false;
        StopCoroutine(FeedbackManager.instance.PlayUtoBattleFeedback());

        // Ubah musik kembali menjadi in-game music
        AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);
        PersistentManager.Instance.UpdateUtoDefeated(true);
        TutorialManager.Instance.StartTutorialAkik();
    }

    public IEnumerator CheckAndActivateCollectButton() {
        speed = 0;
        yield return new WaitForSeconds(0.7f);
        collectDropItemButton.SetActive(true);
    }

    public void SerangButo()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.attackButtonSound);
        if (buttoHealth > 0 && buttoHealth < maxHealth)
        {
            StartIncreasingHealth();
        }
        UpdateButoHealth(-3);
    }

    public void CollectDropItem() // inspector
    {   
        premanButoDropItem.SetActive(false);
        collectDropItemButton.SetActive(false);
        StartCoroutine(FeedbackManager.instance.PlayCollectBatuAkik());
        PersistentManager.Instance.UpdateBatuAkik(1);
        StartCoroutine(DestroyButoAfterDelay());

        TutorialManager.Instance.StartTutorialUpgrade();
    }

    private IEnumerator DestroyButoAfterDelay() {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    // Coroutine untuk menambah health ketika player berada di dalam trigger
    public void StartIncreasingHealth()
    {
        if (increaseHealthCoroutine == null)
        {
            Debug.Log("Memulai Coroutine Penambahan Darah");
            increaseHealthCoroutine = StartCoroutine(IncreaseHealthOverTime());
        }
    }

    public void StopIncreasingHealth()
    {
        if (increaseHealthCoroutine != null)
        {
            StopCoroutine(increaseHealthCoroutine);
            increaseHealthCoroutine = null;

            buttoHealth = 0;
        }
    }

    public void ResetHealthAndStop()
    {
        if (increaseHealthCoroutine != null)
        {
            StopCoroutine(increaseHealthCoroutine);
            increaseHealthCoroutine = null;

            buttoHealth = 60;

            UpdateHealthUI();
        }
    }

    private IEnumerator IncreaseHealthOverTime()
    {
        while (buttoHealth < maxHealth && buttoHealth != 0)
        {
            UpdateButoHealth(5);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator PlayWInFlash()
    {
        if (winFlash.activeSelf != true)
        {
            winFlash.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.winFlashSound);
        }
        else
        {
            winFlash.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        winFlash.SetActive(false);
    }

    private IEnumerator PlayLoseFlash()
    {
        if (winFlash.activeSelf != true)
        {
            winFlash.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.loseFlashSound);
        }
        else
        {
            winFlash.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        winFlash.SetActive(false);
    }

    public void StopMovement()
    {
        // Hanya hentikan coroutine yang terkait dengan pergerakan
        speed = 0;
        isUtoMove = false; // Set animasi berhenti
        StopCoroutine(MoveToTarget());
        StopCoroutine(ButoLoop());
    }

    // baru
    private IEnumerator PauseAtTargetAndCollectCoins() {
        // Berhenti selama 3 detik
        StopMovement();
        yield return new WaitForSeconds(3f);
        StartCoroutine(ButoLoop());
    }

    private bool isButoLoop;
    public IEnumerator ButoLoop() {
        speed = 300;
        isButoLoop = true;

        if (buttoHealth > 0) { 
            if (isButoLoop) // Cek jika Buto belum kalah
            {
                SetRandomTarget();
                yield return StartCoroutine(MoveToTarget());
                isButoLoop = false;
            }
        }
    }

    [SerializeField] private Sprite normalAttack;
    [SerializeField] private Sprite downAttack;
    [SerializeField] private Button attackButton;

    public void OnDownAttack() {
        attackButton.image.sprite = downAttack;
    }

    public void OnNormalAttack() {
        attackButton.image.sprite = normalAttack;
    }


    [SerializeField] private Sprite normalcollect;
    [SerializeField] private Sprite downcollect;
    [SerializeField] private Button collecDropButton;

    public void OnDownCollect() {
        collecDropButton.image.sprite = downcollect;
    }

    public void OnNormalCollect() {
        collecDropButton.image.sprite = normalcollect;
    }
}
