using UnityEngine;
using System.Collections;

public class MobilBakController : MonoBehaviour {
    public Transform targetPosition;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private GameObject blokirWorld;

    private Vector3 velocity = Vector3.zero;

    private void Start() {
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget() {
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f) {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition.position, ref velocity, smoothTime, speed, Time.deltaTime);
            yield return null;
        }
        TutorialManager.Instance.isCutPagiPertama = true;
        TutorialManager.Instance.StartTutorialRestok();
        ScanAstar();
        blokirWorld.SetActive(false);
    }

    private void ScanAstar() {
        GameObject aStarObject = GameObject.Find("A_Star");

        if (aStarObject != null) {
            AstarPath pathfinder = aStarObject.GetComponent<AstarPath>(); // Menggunakan AstarPath
            if (pathfinder != null) {
                pathfinder.Scan(); // Panggil metode Scan
            } else {
                Debug.LogWarning("Komponen AstarPath tidak ditemukan pada GameObject A_Star!");
            }
        } else { 
            Debug.LogWarning("GameObject A_Star tidak ditemukan di scene!");
        }
    }
}