// NOTE ga pake
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerDestroyMerchant : MonoBehaviour {
    public int merchantIndex;
    [SerializeField] private GameObject gfxMerchant;
    [SerializeField] private GameObject canvasWorld;

    private void Start() {
        merchantIndex = MerchantManager.Instance.GetCurrentMerchantIndex();
    }

    // private void OnEnable() {
    //     OnBossAtMerchant += HandleBossAtMerchant;
    // }

    // private event Action OnBossAtMerchant;

    private Dictionary<Collider2D, float> npcTimers = new Dictionary<Collider2D, float>();
    private HashSet<Collider2D> npcProcessed = new HashSet<Collider2D>();
    private float requiredUwoTime = 1f;
    private bool isDebug;

    private void OnTriggerStay2D(Collider2D collision) { // NOTE CEK SCRIPT TriggerDestroyMerchant
        if (collision.CompareTag("UwoTitik") && !npcProcessed.Contains(collision)) {
            if (!isDebug) {
                Debug.LogWarning("Uwo Masuk Trigger");
                isDebug = true;
            }

            if (!npcTimers.ContainsKey(collision)) {
                npcTimers[collision] = 0f;  // Mulai timer untuk NPC baru yang masuk
            }

            npcTimers[collision] += Time.deltaTime;  // Tambah waktu untuk NPC ini

            if (npcTimers[collision] >= requiredUwoTime) {
                // CallBossAtMerchant();
                HandleBossAtMerchant();
                npcTimers.Remove(collision);  // Hapus NPC dari daftar setelah logika dijalankan
                npcProcessed.Add(collision);
            }
        }
    }

    // private void CallBossAtMerchant() {
    //     OnBossAtMerchant?.Invoke();
    // }

    private void HandleBossAtMerchant() {
        StartCoroutine(DestroyAfterDelay());

        // if (merchantIndex >= 0 && merchantIndex < PersistentManager.Instance.dataMerchantList.Count) {
        //     PersistentManager.Instance.dataMerchantList.RemoveAt(merchantIndex);
        // }
    }

    private IEnumerator DestroyAfterDelay() { // NOTE animasi merchant ancur di sini
        // NOTE nni play coroutine, nanti disesuain wait nya
        Debug.LogWarning("Sayur DIPANGGIL");
        StartCoroutine(PlayMerchantAncur());
        yield return new WaitForSeconds(0.5f);
        Destroy(gfxMerchant);
        Destroy(canvasWorld);

        // yield return new WaitForSeconds(1f);
        // Destroy(parentMerchant);
    }

    [Header("Merchant Ancur")]
    [SerializeField] private GameObject merchantAncur;
    [SerializeField] private Sprite[] merchantAncurFrames;
    public IEnumerator PlayMerchantAncur() {
        merchantAncur.SetActive(true);
        for (int i = 0; i < merchantAncurFrames.Length; i++) {
            merchantAncur.GetComponent<SpriteRenderer>().sprite = merchantAncurFrames[i];
            yield return new WaitForSeconds(0.055f);
        }
    }
}
