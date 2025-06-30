using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    public static Timer Instance;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private InvoiceUI invoiceUI;
    [SerializeField] private GameObject blokirWorld;

    public float elapsedTime = 0;
    private float totalInGameMinutes = 12 * 60; // Total waktu 12 jam in-game
    private float realLifeDuration = 2.5f * 60; // Durasi menit di real life dalam detik
    
    public int hours;
    public int minutes;
    
    private void Update() {
        if (TutorialManager.Instance.isTimerStart == true) {
            StartTimer();
            CheckTimer();
        }
    }

    private void StartTimer () {    
        if (PersistentManager.Instance.isPlayTimer == true) {
            elapsedTime += Time.deltaTime;
        }

        // Menghitung waktu in-game berdasarkan rasio real life
        float inGameMinutes = (elapsedTime / realLifeDuration) * totalInGameMinutes;

        // Menghitung jam dan menit berdasarkan waktu in-game
        hours = 18 + Mathf.FloorToInt(inGameMinutes / 60);
        minutes = Mathf.FloorToInt(inGameMinutes % 60);

        // Reset jam menjadi 0 saat melewati 24:00
        if (hours >= 24) {
            hours -= 24;
        }

        timer.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    public event Action OnTutorialTimer; // untuk tutorial waktu
    public event Action OnSpawnUto; // untuk spawn Uto
    public event Action OnDialogUrban;
    public event Action OnMonologUrban;
    public event Action OnSpawnUwoBoss;
    public event Action OnPocinActive;
    public event Action OnPocinDeactive;
    public event Action OnKunchanActive;
    public event Action OnAyangActive;

    private bool isPlayNotifUto;
    private bool isPlayNotifBoss;
    private bool isSpawnBoss;
    private bool isShopNull;

    private void CheckTimer() {
        if (hours == 21 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 1) {
            OnPocinActive?.Invoke();
        } else if (PersistentManager.Instance.dayCounter == 1 && PersistentManager.Instance.nightCounter == 1) {
            OnPocinDeactive?.Invoke();
        }

        if (hours == 21 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 2) {
            OnKunchanActive?.Invoke();
        }

        if (hours == 0 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 2) {
            OnAyangActive?.Invoke();
        }

        if (hours == 21 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 1) {
            OnTutorialTimer?.Invoke();
        }

        if (hours == 0 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 1) {
            if (isPlayNotifUto == false) {
                StartCoroutine(FeedbackManager.instance.PlayNotifUto());
                isPlayNotifUto = true;
            }
        }

        if (hours == 0 && minutes == 28 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 1) {
            OnSpawnUto?.Invoke();
        }

        if (hours == 5 && minutes == 30 && PersistentManager.Instance.isNowMalam) {
            StartCoroutine(FeedbackManager.instance.PlayNotifMalam());
        } else if (!PersistentManager.Instance.isNowMalam) {
            StopCoroutine(FeedbackManager.instance.PlayNotifMalam());
        }

        if (hours == 5 && minutes >= 45) {
            blokirWorld.SetActive(true);
            if (isShopNull == false) {
                MerchantManager.Instance.SetActiveMerchantType(null);
                FurniturManager.Instance.SetActiveFurniturType(null);
                SpesialManager.Instance.SetActiveSpesialType(null);
                isShopNull = true;
            }
        }

        if (hours == 6 && minutes == 0) {
            if (PersistentManager.Instance.isInvoiceShown == false) {
                invoiceUI.ShowInvoice();
                PersistentManager.Instance.isInvoiceShown = true;
                blokirWorld.SetActive(false);
            }
        }

        if (hours == 0 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 2) {
            OnDialogUrban?.Invoke();
        }

        if (hours == 2 && minutes == 40 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 2) {
            OnMonologUrban?.Invoke();
            if (isPlayNotifBoss == false) {
                AudioManager.audioManager.PlaySFX(AudioManager.audioManager.utoNotif);
                isPlayNotifBoss = true;
            }
        }

        if (hours == 3 && minutes == 0 && PersistentManager.Instance.isNowMalam && PersistentManager.Instance.nightCounter == 2) {
            if (isSpawnBoss == false) {
                OnSpawnUwoBoss?.Invoke();
                isSpawnBoss = true;
            }
        }
    }
}
