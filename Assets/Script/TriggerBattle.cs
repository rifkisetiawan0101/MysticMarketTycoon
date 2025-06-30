using System.Collections;
using UnityEngine;

public class TriggerBattle : MonoBehaviour {
    [SerializeField] private GameObject koinUI;
    [SerializeField] private GameObject gemsUI;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject penghargaanUI;
    [SerializeField] private GameObject menuUI;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(CloseAllUI());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(OpenAllUI());
        }
    }

    private IEnumerator CloseAllUI() {
        yield return new WaitForSeconds(3f);
        koinUI.SetActive(false);
        gemsUI.SetActive(false);
        timerUI.SetActive(false);
        shopUI.SetActive(false);
        penghargaanUI.SetActive(false);
        menuUI.SetActive(false);
    }

    private IEnumerator OpenAllUI() {
        yield return new WaitForSeconds(3f);
        koinUI.SetActive(true);
        gemsUI.SetActive(true);
        timerUI.SetActive(true);
        shopUI.SetActive(true);
        penghargaanUI.SetActive(true);
        menuUI.SetActive(true);
    }
}
