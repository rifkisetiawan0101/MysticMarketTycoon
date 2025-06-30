using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;

public class PlayerMovementNewPagi : MonoBehaviour
{
    public float moveSpeed = 1000f;
    public GameObject clickMarkerPrefab;
    public float nextWaypointDistance = 20f;
    public GameObject playerGFX;

    private Rigidbody2D rb;
    private Animator animator;
    private Seeker seeker;
    private Vector2 targetPosition;
    public static bool isMoving = false;
    private Path path;
    private int currentWaypoint = 0;
    private Vector2 velocity;
    private float smoothTime = 0.1f;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        targetPosition = rb.position;
        animator = playerGFX.GetComponent<Animator>();
    }

    private void Update()
    {   
        if (Input.GetMouseButtonDown(1) && (!EventSystem.current.IsPointerOverGameObject())) {
            SetTargetPosition();
            ShowClickMarker();
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.clickJalan);
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MovePlayer();
        }
    }

    void SetTargetPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        // Lakukan raycast untuk memeriksa apakah klik mengenai obstacle
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //Tambahin
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("UI") || hit.collider.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
        {
            targetPosition = new Vector2(mousePos.x, mousePos.y);
            isMoving = true;
            UpdatePath();
        }
        else
        {
            Debug.Log("Klik mengenai obstacle, tidak mengatur target posisi.");
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && Vector2.Distance(rb.position, targetPosition) > nextWaypointDistance)
        {
            seeker.StartPath(rb.position, targetPosition, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void MovePlayer()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            ReachDestination();
            return;
        }

        Vector2 currentWaypointPosition = path.vectorPath[currentWaypoint];
        rb.position = Vector2.SmoothDamp(rb.position, currentWaypointPosition, ref velocity, smoothTime, moveSpeed);

        float distance = Vector2.Distance(rb.position, currentWaypointPosition);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Kondisi untuk memeriksa jika sudah mencapai target
        if (Vector2.Distance(rb.position, targetPosition) < nextWaypointDistance)
        {
            ReachDestination();
        }
    }

    void ReachDestination()
    {
        isMoving = false; // Berhenti bergerak
        rb.velocity = Vector2.zero;
        path = null;
        UpdateAnimation(); // Pastikan animasi diperbarui
    }

    public void StopPlayer()
    {
        isMoving = false; // Berhenti bergerak
        rb.velocity = Vector2.zero; // Menghentikan semua pergerakan
        path = null; // Menghapus path yang sedang diikuti
        UpdateAnimation(); // Pastikan animasi diperbarui untuk menampilkan idle
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        Vector2 movement = isMoving && path != null ? ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized : Vector2.zero;

        if (isMoving && movement != Vector2.zero)
        {
            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);
            animator.SetFloat(_lastHorizontal, movement.x);
            animator.SetFloat(_lastVertical, movement.y);
        }
        else
        {
            // Jika tidak bergerak, set animasi ke idle
            animator.SetFloat(_horizontal, 0);
            animator.SetFloat(_vertical, 0);
        }
    }

    void ShowClickMarker()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 markerPosition = new Vector3(mousePos.x, mousePos.y, 0);
        GameObject marker = Instantiate(clickMarkerPrefab, markerPosition, Quaternion.identity);
        Destroy(marker, 0.50f);
    }
}
