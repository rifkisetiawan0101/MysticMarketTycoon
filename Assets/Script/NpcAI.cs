using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcAI : MonoBehaviour {
    public float speed = 200f;
    public float smoothTime = 0.1f; 
    public float idleTime = 3f; // Waktu berhenti di target
    public float feedbackTime = 0.8f;

    private MerchantManager merchantManager; 
    private Vector3 targetPosition;
    private Vector3 spawnPosition; // Posisi awal spawn NPC

    Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private Vector2 velocity;
    private Vector2 oldPosition;

    public void SetupNPC(MerchantManager manager) {
        // Inisialisasi MerchantManager dan posisi spawn
        merchantManager = manager;
        spawnPosition = transform.position;
        SetRandomTarget();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "InGamePagi") {
            Destroy(gameObject);
        }
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        oldPosition = rb.position;
    }

    private void Start() {
        if (merchantManager != null) {
            SetRandomTarget();
            StartCoroutine(MoveToTarget());
        }

        // if (Timer.Instance.hours == 5 && Timer.Instance.minutes == 50) {
        //     Destroy(gameObject);
        // }

        if (SceneManager.GetActiveScene().name != "InGame") {
            Destroy(gameObject);
        }
    }

    private void Update() {
        NpcAnimation();
    }

    [SerializeField] private int randomIndex;
    
    private void SetRandomTarget() {
        if (PersistentManager.Instance.dataMerchantList.Count > 0) {
            randomIndex = Random.Range(0, PersistentManager.Instance.dataMerchantList.Count);
            targetPosition = PersistentManager.Instance.dataMerchantList[randomIndex].merchantPosition;
        }
    }

    public bool isNpcMove = false;
    private void NpcAnimation() {
        Vector2 newPosition = transform.position;
        Vector2 movement = (newPosition - oldPosition).normalized;
        oldPosition = newPosition;

        if (movement != Vector2.zero) {
            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);
            animator.SetFloat(_lastHorizontal, movement.x);
            animator.SetFloat(_lastVertical, movement.y);
        if (isNpcMove) {
                animator.Play("Movement");
            }
        } else {
            if (!isNpcMove) {
                animator.Play("Idle");
            }
        }
    }

    private IEnumerator MoveToTarget() {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed);
            rb.MovePosition(targetPos);
            isNpcMove = true;

            yield return null;
        }

        StartCoroutine(idleAtTarget());
    }

    private IEnumerator MoveToTarget2() {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed);
            rb.MovePosition(targetPos);

            isNpcMove = true;
            
            yield return null;
        }

        StartCoroutine(idleAtTarget2());
    }

    private IEnumerator idleAtTarget() {
        yield return new WaitForSeconds(idleTime);
        var merchantData = PersistentManager.Instance.dataMerchantList[randomIndex];

        if (merchantData.stokDagangan > 0) {
            isNpcMove = false;
        }
        
        yield return new WaitForSeconds(feedbackTime);
        StartCoroutine(MoveRandomly());
    }

    private IEnumerator idleAtTarget2() {
        yield return new WaitForSeconds(idleTime);
        var merchantData = PersistentManager.Instance.dataMerchantList[randomIndex];

        if (merchantData.stokDagangan > 0) {
            isNpcMove = false;
        }

        yield return new WaitForSeconds(feedbackTime);
        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator idleAtRandom() {
        isNpcMove = true;
        yield return new WaitForSeconds(idleTime);
        SetRandomTarget();
        StartCoroutine(MoveToTarget2());
    }

    private IEnumerator MoveRandomly() {
        Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(1500f, 3000f);
        Vector3 randomTarget = transform.position + randomDirection;

        while (Vector3.Distance(transform.position, randomTarget) > 0.1f) {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, randomTarget, ref velocity, smoothTime, speed);
            rb.MovePosition(targetPos);

            isNpcMove = true;

            yield return null;
        }
        StartCoroutine(idleAtRandom());
    }

    private IEnumerator MoveToSpawn() {
        while (Vector3.Distance(transform.position, spawnPosition) > 0.1f) {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, spawnPosition, ref velocity, smoothTime, speed);
            rb.MovePosition(targetPos);

            isNpcMove = true;

            yield return null;
        }

        Destroy(gameObject); // Menghancurkan NPC setelah kembali ke posisi awal
        PersistentManager.Instance.dataTotalSpawnNpc--;
    }
}
