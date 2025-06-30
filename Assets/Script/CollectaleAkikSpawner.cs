using UnityEngine;

public class CollectableAkikSpawner : MonoBehaviour {
    public float spawnRadius = 1800f;
    public GameObject akikPrefab;
    private void Start() {
        if (PersistentManager.Instance.dayCounter >= 1) {
            SpawnAkik(akikPrefab);
            SpawnAkik(akikPrefab);
            SpawnAkik(akikPrefab);
        }
    }
    private void SpawnAkik (GameObject prefab) {
        Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
