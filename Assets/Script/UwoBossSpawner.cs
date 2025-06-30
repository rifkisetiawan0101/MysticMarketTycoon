using UnityEngine;
using UnityEngine.SceneManagement;

public class UwoBossSpawner : MonoBehaviour
{
    public GameObject uwoBossPrefabs;
    public MerchantManager merchantManager;

    private GameObject currentNPC;
    [SerializeField] private Timer timer;

    private void HandleSpawnUwoBoss()
    {
        if (!PersistentManager.Instance.isNowMalam || PersistentManager.Instance.nightCounter < 2)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            SpawnNPC();

            //if (TutorialManager.Instance.isUtoSpawn == false)
            //{
            //    TutorialManager.Instance.StartTutorialUto();
            //    TutorialManager.Instance.isUtoSpawn = true;
            //}
        }
    }

    private void SpawnNPC()
    {
        currentNPC = Instantiate(uwoBossPrefabs, transform.position, Quaternion.identity);
        UwoBossAI uwoBossAI = currentNPC.GetComponent<UwoBossAI>();
        uwoBossAI.SetupNPC(merchantManager); // Kirim referensi MerchantManager ke NPC
    }

    private void OnEnable()
    {
        if (timer != null)
        {
            timer.OnSpawnUwoBoss += HandleSpawnUwoBoss;
        }
        else
        {
            Debug.LogWarning("Timer reference is missing in PremanSpawner.");
        }
    }
}
