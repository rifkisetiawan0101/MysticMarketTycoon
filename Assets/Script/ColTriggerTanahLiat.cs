using System.Collections;
using UnityEngine;

public class ColTriggerTanahLiat : MonoBehaviour {
    [SerializeField] private GameObject collectCanvas;
    [SerializeField] private GameObject teksTanahLiat;
    [SerializeField] private GameObject blokirWorld;
    public static bool isPlayerInRange = false;

    private void Start() {
        collectCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            collectCanvas.SetActive(true);
            teksTanahLiat.SetActive(true);
            isPlayerInRange = true;

            if (TutorialManager.Instance.isTutorialRestokPlayed) {
                TutorialManager.Instance.isNearItem = true;
            }

            StartCoroutine(DelayBlokir());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            collectCanvas.SetActive(false);
            teksTanahLiat.SetActive(true);
            isPlayerInRange = false;
            CollectTanahLiat.holdTimer = 0f;  // Reset timer saat pemain keluar

            blokirWorld.SetActive(false);
        }
    }

    private IEnumerator DelayBlokir() {
        blokirWorld.SetActive(true);
        yield return new WaitForSeconds(3f);
        blokirWorld.SetActive(false);
    }
}
