using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SetMainCamera : MonoBehaviour {
    private Canvas canvas;
    private Camera mainCamera;

    private void Start() {
        canvas = GetComponent<Canvas>();
        AssignMainCamera();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(WaitAndAssignCamera());
    }

    private IEnumerator WaitAndAssignCamera() {
        yield return null; // Tunggu 1 frame agar main camera sudah ter-load
        AssignMainCamera();
    }

    private void AssignMainCamera() {
        mainCamera = Camera.main;
        if (mainCamera != null) {
            canvas.worldCamera = mainCamera;
        } else {
            Debug.LogWarning("Main Camera tidak ditemukan di scene ini.");
        }
    }
}
