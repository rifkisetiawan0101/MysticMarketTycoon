using System.Collections.Generic;
using UnityEngine;

public class UwoBossAttackTrigger : MonoBehaviour
{
    [SerializeField] public GameObject serangUwoButton;
    [SerializeField] private UwoBossAI uwoBossAI;

    public bool isPlayerEnterTrigger = false;
    private bool isPlayBattle = false;

    void Start()
    {
        serangUwoButton.SetActive(false); // Awalnya dimatikan
        uwoBossAI.overlay.SetActive(false);

        Debug.Log("isPlayerEnterTrigger: " + isPlayerEnterTrigger);
    }

    // Fungsi untuk mengaktifkan tombol serang
    public void ActivateSerangButton()
    {
        if (serangUwoButton != null && uwoBossAI.buttoHealth > 0 && uwoBossAI.buttoHealth != uwoBossAI.maxHealth && PersistentManager.Instance.isUIOpen == false)
        {
            serangUwoButton.SetActive(true);
            uwoBossAI.overlay.gameObject.SetActive(true);
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.battleBacksound, 0.5f);

            PersistentManager.Instance.isBattleBoss = true;

            if (isPlayBattle == false) {
                StartCoroutine(FeedbackManager.instance.PlayBossBattleFeedback());
                isPlayBattle = true;
            }
        }
    }

    private Dictionary<Collider2D, float> playerTimers = new Dictionary<Collider2D, float>();
    public float requiredTime = 2f;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!playerTimers.ContainsKey(collision))
            {
                playerTimers[collision] = 0f; // Mulai timer untuk player yang masuk
            }

            playerTimers[collision] += Time.deltaTime; // Tambah waktu untuk player ini
            if (playerTimers[collision] >= requiredTime)
            {   
                if (uwoBossAI.buttoHealth > 0)
                {   
                    ActivateSerangButton(); // Aktifkan tombol serang
                    uwoBossAI.StopMovement();
                }

                playerTimers.Remove(collision);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerEnterTrigger = false;
            serangUwoButton.SetActive(false); // Menonaktifkan GameObject SerangButoButton
            uwoBossAI.overlay.gameObject.SetActive(false);
            uwoBossAI.ResetHealthAndStop();
            uwoBossAI.buttoHealth = 60;
            StartCoroutine(uwoBossAI.ButoLoop());

            PersistentManager.Instance.isBattleBoss = false;
            StopCoroutine(FeedbackManager.instance.PlayBossBattleFeedback());

            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);

            isPlayBattle = false;
            
            playerTimers.Remove(collision);
        }
    }
}