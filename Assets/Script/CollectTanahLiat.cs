using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectTanahLiat : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private GameObject tanahLiatPrefab;
    [SerializeField] private float holdTime = 1f;
    [SerializeField] private Button buttonTapHold;
    [SerializeField] private GameObject tapHold;
    [SerializeField] private GameObject blokirWorld;
    private Animator tapHoldAnimator;
    public static float holdTimer = 0f;
    private bool isHolding = false;
    private bool hasCollected = false;

    private void Start() {
        tapHold.SetActive(false);
        tapHoldAnimator = tapHold.GetComponent<Animator>();
    }

    private void Update() {
        if (isHolding && ColTriggerTanahLiat.isPlayerInRange & !hasCollected) {
            holdTimer += Time.deltaTime;
            blokirWorld.SetActive(true);

            if (holdTimer >= holdTime) {
                hasCollected = true;
                tapHold.SetActive(false);
                PersistentManager.Instance.UpdateCollectable(1, "TanahLiat");
                TutorialManager.Instance.isItemCollected = true;
                StartCoroutine(FeedbackManager.instance.PlayTanahCollect(tanahLiatPrefab));
            }
        }
    }

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnPointerDown(PointerEventData eventData) {
        if (ColTriggerTanahLiat.isPlayerInRange) {
            isHolding = true;  // Mulai hold
            buttonTapHold.image.sprite = highlightedSprite;
            tapHold.SetActive(true);
            tapHoldAnimator.Play("TapHoldAnim");

            FindObjectOfType<PlayerMovementNewPagi>().StopPlayer();
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        isHolding = false;  // Berhenti hold
        holdTimer = 0f;
        buttonTapHold.image.sprite = normalSprite;
        tapHold.SetActive(false);
    }
}
