using UnityEngine;

public class DontDestroyCamera : MonoBehaviour {
    public static DontDestroyCamera Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }
}
