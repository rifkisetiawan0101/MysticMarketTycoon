using UnityEngine;

public class TriggerOpenInfo : MonoBehaviour {
    [SerializeField] private GameObject openInfo;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            openInfo.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            openInfo.SetActive(false);
        }
    }
}
