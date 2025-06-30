using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcSpawner : MonoBehaviour {    
    public GameObject npcPrefabs;
    public float minSpawnTime = 3f; // Waktu spawn minimum
    public float maxSpawnTime = 6f; // Waktu spawn maksimum
    public MerchantManager merchantManager; // Reference ke MerchantManager untuk akses targetMerchantNPCList
    private GameObject npc;
    private void Start() {
        if (SceneManager.GetActiveScene().name == "InGame") {
            StartCoroutine(SpawnNPC());
        } else {
            StopCoroutine(SpawnNPC());
        }
    }

    IEnumerator SpawnNPC() {
        while (true) {
            if (PersistentManager.Instance.dataTotalSpawnNpc < PersistentManager.Instance.dataMaxNpc && PersistentManager.Instance.isNowMalam && TutorialManager.Instance.isTutorialFurniturStatuePlayed) {
                float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(spawnTime);

                npc = Instantiate(npcPrefabs, transform.position, Quaternion.identity);
                NpcAI npcAI = npc.GetComponent<NpcAI>();
                npcAI.SetupNPC(merchantManager); // Kirim referensi MerchantManager ke NPC

                PersistentManager.Instance.dataTotalSpawnNpc++;
                TutorialManager.Instance.isNpcSpawn = true;
            }
            yield return null;
            if (SceneManager.GetActiveScene().name != "InGame") {
                Destroy(npc);
            }
            yield return null;
        }
    }
}
