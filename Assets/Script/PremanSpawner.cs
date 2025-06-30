using UnityEngine;
using UnityEngine.SceneManagement;

public class PremanSpawner : MonoBehaviour
{
    public GameObject premanPrefabs;
    public MerchantManager merchantManager;

    private GameObject currentNPC;
    [SerializeField] private Timer timer;

    private void HandleSpawnUto() {
        if (!PersistentManager.Instance.isNowMalam || PersistentManager.Instance.nightCounter < 1 || PersistentManager.Instance.nightCounter > 1) {
            return;
        }

        if (SceneManager.GetActiveScene().name == "InGame") {
            SpawnNPC();
            
            if (TutorialManager.Instance.isUtoSpawn == false) {
                TutorialManager.Instance.StartTutorialUto();
                TutorialManager.Instance.isUtoSpawn = true;
            }    
        }
    }

    private void SpawnNPC()
    {
        currentNPC = Instantiate(premanPrefabs, transform.position, Quaternion.identity);
        PremanButoAI premanButoAI = currentNPC.GetComponent<PremanButoAI>();
        premanButoAI.SetupNPC(merchantManager); // Kirim referensi MerchantManager ke NPC
    }

    private void OnEnable() {
        if (timer != null) {
            timer.OnSpawnUto += HandleSpawnUto;
        } else {
            Debug.LogWarning("Timer reference is missing in PremanSpawner.");
        }
    }
}
