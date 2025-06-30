using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanduanController : MonoBehaviour
{
    [SerializeField] private GameObject[] panduanPanels;
    [SerializeField] private Button[] panduanButtons;

    [SerializeField] private Sprite[] activeSprites;
    [SerializeField] private Sprite[] defaultSprites; 

    private void Start()
    {
        // Menambahkan listener untuk setiap button
        for (int i = 0; i < panduanButtons.Length; i++)
        {
            int index = i; // Menyimpan index dalam variabel lokal untuk digunakan di dalam listener
            panduanButtons[i].onClick.AddListener(() => OnButtonClick(index));

            // Menambahkan event hover menggunakan EventTrigger
            EventTrigger trigger = panduanButtons[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnButtonHover(index); });
            trigger.triggers.Add(entry);
        }

        // Aktifkan panel pertama dan ubah tombol pertama ke sprite aktif saat awal
        OnButtonClick(0);
    }

    private void OnButtonClick(int index)
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);

        // Nonaktifkan semua panel
        foreach (GameObject panel in panduanPanels)
        {
            panel.SetActive(false);
        }

        // Aktifkan panel yang sesuai berdasarkan tombol yang diklik
        if (index >= 0 && index < panduanPanels.Length)
        {
            panduanPanels[index].SetActive(true);
        }

        // Kembalikan semua tombol ke sprite default
        for (int i = 0; i < panduanButtons.Length; i++)
        {
            panduanButtons[i].image.sprite = defaultSprites[i];
        }

        // Ubah sprite tombol yang diklik menjadi sprite aktif
        panduanButtons[index].image.sprite = activeSprites[index];
    }

    private void OnButtonHover(int index)
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }
}
