using System.Collections.Generic;
using UnityEngine;

public class PremanButoAttackTrigger : MonoBehaviour
{
    [SerializeField] public GameObject serangButoButton;
    [SerializeField] private GameObject collectDropItemButton;
    [SerializeField] private PremanButoAI premanButoAI;

    public bool isPlayerEnterTrigger = false;
    private bool isPlayBattle = false;

    void Start()
    {
        serangButoButton.SetActive(false); // Awalnya dimatikan
        collectDropItemButton.SetActive(false);
        premanButoAI.overlay.SetActive(false);
        
        Debug.Log("isPlayerEnterTrigger: " + isPlayerEnterTrigger);
    }

    // Fungsi untuk mengaktifkan tombol serang
    public void ActivateSerangButton()
    {
        if (serangButoButton != null && premanButoAI.buttoHealth > 0 && premanButoAI.buttoHealth != premanButoAI.maxHealth && PersistentManager.Instance.isUIOpen == false)
        {
            Debug.Log("ActivateSerangButton dipanggil");
            serangButoButton.SetActive(true);
            premanButoAI.overlay.gameObject.SetActive(true);
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.battleBacksound, 0.5f);

            PersistentManager.Instance.isBattleUto = true;
            if (isPlayBattle == false) {
                StartCoroutine(FeedbackManager.instance.PlayUtoBattleFeedback());
                isPlayBattle = true;
            }
        }
    }

    private Dictionary<Collider2D, float> playerTimers = new Dictionary<Collider2D, float>();
    public float requiredTime = 1f;
    void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (!playerTimers.ContainsKey(collision)) {
                playerTimers[collision] = 0f; // Mulai timer untuk player yang masuk
            }

            playerTimers[collision] += Time.deltaTime; // Tambah waktu untuk player ini
            if (playerTimers[collision] >= requiredTime) {
                if (premanButoAI.buttoHealth > 0) {
                    ActivateSerangButton(); // Aktifkan tombol serang
                    PremanButoAI.Instance.isUtoMove = false;
                    premanButoAI.StopMovement();
                }
                playerTimers.Remove(collision);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            isPlayerEnterTrigger = false;
            serangButoButton.SetActive(false); // Menonaktifkan GameObject SerangButoButton
            premanButoAI.overlay.gameObject.SetActive(false);
            premanButoAI.ResetHealthAndStop();
            premanButoAI.buttoHealth = 60;
            StartCoroutine(premanButoAI.ButoLoop());
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);

            if (premanButoAI.buttoHealth <= 0 && premanButoAI.premanButoDropItem.activeSelf) {
                Debug.Log("OnTriggerExit2D: Menonaktifkan tombol collect");
                premanButoAI.collectDropItemButton.SetActive(false);
            }

            PersistentManager.Instance.isBattleUto = false;
            StopCoroutine(FeedbackManager.instance.PlayUtoBattleFeedback());
            
            isPlayBattle = false;

            playerTimers.Remove(collision);
        }
    }
}