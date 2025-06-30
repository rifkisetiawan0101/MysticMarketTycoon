using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    public GameObject batuPrefab;
    public GameObject kayuPrefab;
    public GameObject tanahLiatPrefab;
    
    public int minSpawnCount = 2;
    public int maxSpawnCount = 4;
    public float spawnRadius = 1000f;

    void Start()
    {
        SpawnCollectables(batuPrefab);
        SpawnCollectables(kayuPrefab);
        SpawnCollectables(tanahLiatPrefab);
    }

    void SpawnCollectables(GameObject prefab)
    {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
