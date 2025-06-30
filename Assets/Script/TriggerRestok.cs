using UnityEngine;

public class TriggerRestok : MonoBehaviour {
    [SerializeField] private Canvas restokCanvas;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            restokCanvas.gameObject.SetActive(true); // Tampilkan canvas collect
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            restokCanvas.gameObject.SetActive(false); // Sembunyikan canvas collect
        }
    }
}
