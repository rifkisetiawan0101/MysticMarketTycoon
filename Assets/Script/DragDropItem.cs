using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DragDropItem Instance;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;
    public bool isOverBakul;
    private GameObject bakulNormal;
    private MinigamePocin minigamePocin; // Tambahkan variabel ini

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        bakulNormal = GameObject.FindWithTag("BakulNormal"); // Temukan bakul dengan tag BakulNormal
        minigamePocin = FindObjectOfType<MinigamePocin>(); // Temukan MinigamePocin
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.position; // Simpan posisi awal
        canvasGroup.blocksRaycasts = false; // Agar item tidak menghalangi raycast saat di-drag
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.dragStartSound); // Play SFX saat drag dimulai (Opsional)

        minigamePocin.bakulGlow.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor; // Menggerakkan item mengikuti posisi mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Kembalikan blokir raycast setelah dragging selesai

        if (isOverBakul) // Jika item berada di atas bakul
        {
            Debug.Log("itemMasuk");
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.miniGameClick);

            minigamePocin.ItemMasukKeBakul();

            minigamePocin.bakulGlow.SetActive(true); 
            Invoke("MatikanBakulGlow", 0.15f);

            this.enabled = false;
        }
        else // Jika tidak berada di bakul, kembalikan ke posisi awal
        {
            rectTransform.position = initialPosition;
        }

        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.dragEndSound);
    }

    // Metode untuk mendeteksi ketika item memasuki bakul
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BakulNormal")) // Pastikan collider yang disentuh memiliki tag "BakulNormal"
        {
            isOverBakul = true;
            Debug.Log("Masuk ke collider bakul");
        }
    }

    // Metode untuk mendeteksi ketika item keluar dari bakul
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BakulNormal")) // Pastikan collider yang disentuh memiliki tag "BakulNormal"
        {
            isOverBakul = false;
            Debug.Log("Keluar dari collider bakul");
        }
    }
}
