using UnityEngine;
using TMPro;

public class KoinUI : MonoBehaviour {
    public TextMeshProUGUI teksKoinUI;

    private void Update() {
        teksKoinUI.text = PersistentManager.Instance.dataKoin.ToString("N0") + "K";
    }
}
