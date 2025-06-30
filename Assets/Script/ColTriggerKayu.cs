using System.Collections;
using UnityEngine;

public class ColTriggerKayu : MonoBehaviour {
    [SerializeField] private GameObject collectCanvas;
    [SerializeField] private GameObject teksKayu;
    [SerializeField] private GameObject blokirWorld;
    public static bool isPlayerInRange = false;

    private void Start() {
        collectCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            collectCanvas.SetActive(true);
            teksKayu.SetActive(true);
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
            teksKayu.SetActive(true);
            isPlayerInRange = false;
            CollectKayu.holdTimer = 0f;  // Reset timer saat pemain keluar

            blokirWorld.SetActive(false);
        }
    }

    private IEnumerator DelayBlokir() {
        blokirWorld.SetActive(true);
        yield return new WaitForSeconds(3f);
        blokirWorld.SetActive(false);
    }
}
