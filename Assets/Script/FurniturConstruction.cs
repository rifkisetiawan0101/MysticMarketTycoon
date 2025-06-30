using UnityEngine;

public class FurniturConstruction : MonoBehaviour {
    [SerializeField] private Transform targetPrefab;
    private float timeToConstruct = 2f;
    private float contructionTimer;
    private bool isPlayingFurniturHiasan;
    private void Update() {
        if (!isPlayingFurniturHiasan) {
            isPlayingFurniturHiasan = true;
            StartCoroutine(FeedbackManager.instance.PlayFurniturHiasan(gameObject));
        }

        contructionTimer += Time.deltaTime;

        if (contructionTimer >= timeToConstruct) {
            Instantiate(targetPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
