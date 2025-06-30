using Pathfinding;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UwoBossAI : MonoBehaviour
{
    public static UwoBossAI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void 
    OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name != "InGame")
        {
            Destroy(gameObject);
        }
    }

    private Seeker seeker;

    [SerializeField] private GameObject premanButoGFX; // Menyimpan referensi ke GFX
    [SerializeField] private GameObject winFlash;
    [SerializeField] private GameObject blockInput;
    [SerializeField]
    private GameObject sihirNotif;
    [SerializeField] private Image blueBarImage;

    private GameObject player;
    private GameObject movePosisiKalah;

    public GameObject overlay;
    public float buttoHealth = 60f; // Health awal 30
    public float maxHealth = 100f; // Health maksimal

    public float speed = 200;
    public bool isPremanArrived = false;
    public bool isBattle = false;
    public bool isUtoMove = false;
    public bool isFirstBattle = true;
    public bool isFirstDefeat = true;
    public bool stopButoIfMerchantZero = false;

    private MerchantManager merchantManager;
    private Vector3 targetPosition;
    private Vector3 spawnPosition; // Posisi awal spawn NPC
    private UwoBossAttackTrigger premanButoAttackTrigger;

    Rigidbody2D rb;
    [SerializeField] private  Animator animator;
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
        premanButoAttackTrigger = GetComponentInChildren<UwoBossAttackTrigger>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        seeker = GetComponent<Seeker>();
        _oldPosition = rb.position;

        koin_1.SetActive(false);
        koin_2.SetActive(false);
        koin_3.SetActive(false);
        koin_4.SetActive(false);

        koin_1.GetComponent<Button>().onClick.AddListener(() => {
            Collect50000Koin(koin_1);
            StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
        });
        koin_2.GetComponent<Button>().onClick.AddListener(() => {
            Collect50000Koin(koin_2);
            StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
        });
        koin_3.GetComponent<Button>().onClick.AddListener(() => {
            Collect50000Koin(koin_3);
            StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
        });
        koin_4.GetComponent<Button>().onClick.AddListener(() => {
            Collect50000Koin(koin_4);
            StartCoroutine(FeedbackManager.instance.PlayCollectKoin());
        });

        StartCoroutine(KoinGIF_1());
        StartCoroutine(KoinGIF_2());
        StartCoroutine(KoinGIF_3());
        StartCoroutine(KoinGIF_4());

        if (merchantManager != null) {
            SetRandomTarget();
            StartCoroutine(MoveToTarget());
        } else {
            Debug.LogWarning("MerchantManager belum di-set.");
        }

        UpdateHealthUI(); // Update UI saat starts

        if (SceneManager.GetActiveScene().name != "InGame")
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private bool isGameOverPlayed;
    private void Update() {
        if (PersistentManager.Instance.isBossDefeated == false) {
            NpcAnimation();
        }
        
        if (totalKoinCollected == 4 && !isGameOverPlayed) {
            TutorialManager.Instance.StartHandleMonologGameOver();
            PersistentManager.Instance.isBossDefeated = true;
            isGameOverPlayed = true;
            StartCoroutine(DestroyBossAfterDelay());
        }
    }

    private void NpcAnimation()
    {
        Vector2 newPosition = transform.position;
        Vector2 movement = (newPosition - _oldPosition).normalized;
        _oldPosition = newPosition;

        if (movement != Vector2.zero)
        {
            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);
            animator.SetFloat(_lastHorizontal, movement.x);
            animator.SetFloat(_lastVertical, movement.y);

            if (isUtoMove)
            {
                animator.Play("Walk");
            }
        }
        else
        {
            if (!isUtoMove)
            {
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
        PersistentManager.Instance.isPlayTimer = false;
        isUtoMove = true;
        speed = 200;
        //PersistentManager.Instance.isBossAtMerchant = false; // gantih
        //PersistentManager.Instance.UpdateBossAtMerchant(false);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 targetPos = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(targetPos);
            _oldPosition = rb.position;

            yield return null;
        }

        isPremanArrived = true; // Menandakan bahwa Buto sudah sampai di lokasi target
        isUtoMove = false;

        StartCoroutine(PauseAtTarget());
    }

    // ------------------------------------- BARU ------------------------------------------------------------ //

    public void UpdateButoHealth(float amount)
    {
        // Update health dan pastikan tidak kurang dari 0 atau lebih dari maxHealth
        buttoHealth = Mathf.Clamp(buttoHealth + amount, 0, maxHealth);

        Debug.Log("Update Health UI: " + buttoHealth);

        UpdateHealthUI();

        if (buttoHealth <= 0) {
            HandleButoDefeated();
        }
        else if (buttoHealth >= maxHealth) // preman buto menang // rojak kalah
        {
            StartCoroutine(PlayLoseFlash());
            isBattle = false;
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);

            if (isFirstBattle == true) {
                isFirstBattle = false;
                StartCoroutine(StartBlockInput());
                StartCoroutine(FeedbackManager.instance.PlaySihirFeedback());
            }

            player.transform.position = movePosisiKalah.transform.position;

            ResetHealthAndStop();
            buttoHealth = 60; // Reset darah ke 60
            overlay.SetActive(false);
            premanButoAttackTrigger.serangUwoButton.SetActive(false);
            StartCoroutine(ButoLoop());

            PersistentManager.Instance.isBattleBoss = false;
            StopCoroutine(FeedbackManager.instance.PlayBossBattleFeedback());
        }
    }

    private void UpdateHealthUI()
    {
        // premanButoHealthUI.text = "HP: " + buttoHealth.ToString("F0");
        blueBarImage.fillAmount = buttoHealth / maxHealth;
    }
    
    [SerializeField] private GameObject koin_1;
    [SerializeField] private GameObject koin_2;
    [SerializeField] private GameObject koin_3;
    [SerializeField] private GameObject koin_4;
    // uwo kalah
    private void HandleButoDefeated() // rojak menang
    {
        StartCoroutine(UwoDeadDestroyGFX());
        premanButoAttackTrigger.serangUwoButton.SetActive(false);
        premanButoAttackTrigger.GetComponent<CircleCollider2D>().enabled = false;
        // premanButoHealthUI.text = " ";
        buttoHealth = 0;
        speed = 0;
        premanButoAttackTrigger.isPlayerEnterTrigger = false;
        overlay.SetActive(false);
        isBattle = false;

        StartCoroutine(PlayWInFlash());
        StartCoroutine(ActiveKoinAfterDelay());
        StopMovement();

        PersistentManager.Instance.isBattleBoss = false;
        StopCoroutine(FeedbackManager.instance.PlayBossBattleFeedback());

        // Ubah musik kembali menjadi in-game music
        AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);
    }

    [Header("Uwo Dead Destroy GFX")]
    [SerializeField] private GameObject uwoDeadAnim;
    [SerializeField] private Sprite[] uwoDeadAnimFrames;
    public IEnumerator UwoDeadDestroyGFX()
    {
        premanButoGFX.SetActive(false);
        uwoDeadAnim.SetActive(true);
        for (int i = 0; i < uwoDeadAnimFrames.Length; i++)
        {
            uwoDeadAnim.GetComponent<SpriteRenderer>().sprite = uwoDeadAnimFrames[i];
            yield return new WaitForSeconds(0.055f);
        }
        uwoDeadAnim.SetActive(false);
    }

    private IEnumerator ActiveKoinAfterDelay() {
        yield return new WaitForSeconds(0.5f);
        koin_1.SetActive(true);
        koin_2.SetActive(true);
        koin_3.SetActive(true);
        koin_4.SetActive(true);
    }

    private int totalKoinCollected = 0;
    private void Collect50000Koin(GameObject koin) {
        totalKoinCollected++;
        PersistentManager.Instance.UpdateKoin(50000);
        koin.SetActive(false);
        Destroy(koin);
    }

    [SerializeField] private Sprite[] gifFrames;
    private float frameDelay = 0.083f;
    private int currentFrame;
    private IEnumerator KoinGIF_1() {
        while (true) {
            koin_1.GetComponent<Image>().sprite = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private IEnumerator KoinGIF_2() {
        while (true) {
            koin_2.GetComponent<Image>().sprite = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private IEnumerator KoinGIF_3() {
        while (true) {
            koin_3.GetComponent<Image>().sprite = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private IEnumerator KoinGIF_4() {
        while (true) {
            koin_4.GetComponent<Image>().sprite = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    public void SerangButo()
    {
        if (isFirstBattle == true)
        {
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.attackButtonSound);
            if (buttoHealth > 0 && buttoHealth < maxHealth)
            {
                StartFirstIncreasingHealth();
            }
            UpdateButoHealth(-2f);
        } 
        else
        { // battle kedua
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.attackButtonSound);
            if (buttoHealth > 0 && buttoHealth < maxHealth)
            {
                StartIncreasingHealth();
            }
            UpdateButoHealth(-3);

            if (buttoHealth >= maxHealth) {
                isFirstDefeat = false;
            }
        }
    }

    private IEnumerator DestroyBossAfterDelay() {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    // Coroutine untuk menambah health ketika player berada di dalam trigger
    public void StartFirstIncreasingHealth()
    {
        if (increaseHealthCoroutine == null)
        {
            Debug.Log("Memulai Coroutine Penambahan Darah");
            increaseHealthCoroutine 
            = StartCoroutine(FirstIncreaseHealthOverTime());
        }
    }

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

    private IEnumerator FirstIncreaseHealthOverTime()
    {
        while (buttoHealth < maxHealth && buttoHealth != 0)
        {
            UpdateButoHealth(20);
            yield return new WaitForSeconds(1f);
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
        StopCoroutine("MoveToTarget");
        StopCoroutine("ButoLoop");
        isUtoMove = false; // Set animasi berhenti
    }

    // baru
    private IEnumerator PauseAtTarget() { // NOTE bug Pause At Target di Target Merchant kedua, kemungkinan dari ButoLoop 
        // Berhenti selama 2 detik
        StopMovement();
        // yield return new WaitForSeconds(1.66f);
        //PersistentManager.Instance.isBossAtMerchant = true; // gantih
        //PersistentManager.Instance.UpdateBossAtMerchant(true);
        // yield return new WaitForSeconds(0.34f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(ButoLoop());

        // isMerchantDestroyed = true;
        // if (isMerchantDestroyed) {
        //     StartCoroutine(ScanAstarAfterWait(8f));
        // }
    }

    private bool isBossLoop;
    public IEnumerator ButoLoop()
    {
        speed = 150;
        isBossLoop = true;

        if (buttoHealth > 0) // Buto terus bergerak sampai dikalahkan  // rojak kalah
        {
            //if (isFirstDefeat == true) {
            //    SetRandomTarget();
            //    StartCoroutine(MoveToTarget());

            //}
            //yield return null;
            if (isBossLoop == true) {
                SetRandomTarget();
                yield return StartCoroutine(MoveToTarget());
                yield return StartCoroutine(PauseAtTarget());
                isBossLoop = false;
            }
            
        }
    }

    // public IEnumerator ButoLoop()  // NOTE sebelumnya begini
    // {
    //     speed = 400;

    //     while (buttoHealth > 0) // Buto terus bergerak sampai dikalahkan  // rojak kalah
    //     {   
    //         if (isFirstDefeat == true) {
    //             SetRandomTarget();
    //             yield return StartCoroutine(MoveToTarget());
    //         }
    //     }
    // }

    // private bool isMerchantDestroyed;
    // private IEnumerator ScanAstarAfterWait(float waitTime) {   
    //     isMerchantDestroyed = false;

    //     GameObject aStarObject = GameObject.Find("A_Star");
    //     yield return new WaitForSeconds(waitTime);

    //     if (aStarObject != null) {
    //         AstarPath pathfinder = aStarObject.GetComponent<AstarPath>(); // Menggunakan AstarPath
    //         if (pathfinder != null) {
    //             pathfinder.Scan(); // Panggil metode Scan
    //         } else {
    //             Debug.LogWarning("Komponen AstarPath tidak ditemukan pada GameObject A_Star!");
    //         }
    //     } else {
    //         Debug.LogWarning("GameObject A_Star tidak ditemukan di scene!");
    //     }
    // }

    [SerializeField] private Sprite normalAttack;
    [SerializeField] private Sprite downAttack;
    [SerializeField] private Button attackButton;

    public void OnDownAttack()
    {
        attackButton.image.sprite = downAttack;
    }

    public void OnNormalAttack()
    {
        attackButton.image.sprite = normalAttack;
    }

    private IEnumerator StartBlockInput()
    {   
        blockInput.SetActive(true);
        yield return new WaitForSeconds(8f);
        blockInput.SetActive(false);
    }

    public void StopButoLoop()
    {
        // Hentikan Buto dari bergerak dan hentikan animasi
        if (increaseHealthCoroutine != null)
        {
            StopCoroutine(increaseHealthCoroutine);
            increaseHealthCoroutine = null;
        }

        StopCoroutine("ButoLoop"); // Hentikan coroutine ButoLoop

        // Set animasi Buto ke Idle, jika ada
        isUtoMove = false;
        if (animator != null)
        {
            animator.Play("Idle");
        }

        Debug.Log("Buto loop dihentikan");
    }

    // private IEnumerator HandleSecondDefeat()
    // {
    //     Debug.LogWarning("handle second defeat dipanggil");
    //     StopButoLoop();
    //     StopMovementKedua(); // Uto berhrnti dan ulti

    //     yield return new WaitForSeconds(3f);

    //     foreach (var merchantData in PersistentManager.Instance.dataMerchantList)
    //     {
    //         Destroy(merchantData.merchantObject);
    //         PersistentManager.Instance.dataMerchantList.Clear();
    //         Debug.LogWarning("all merchant destroyed: " + merchantData.merchantTypeSO);
    //     }
    // }
}