using System.Collections;
using UnityEngine;

public class DontDestroyCanvasfade : MonoBehaviour {
    public static DontDestroyCanvasfade Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private void Start() {
        StartCoroutine(DisableCanvasAfterDelay());
    }

    private IEnumerator DisableCanvasAfterDelay() {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
