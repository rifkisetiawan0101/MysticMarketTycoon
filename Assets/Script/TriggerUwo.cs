using UnityEngine;

public class TriggerUwo : MonoBehaviour {
    [SerializeField] private GameObject WindowUwo;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            WindowUwo.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            WindowUwo.SetActive(false);
        }
    }

}
