using UnityEngine;
using TMPro;

public class TasUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI teksDataSayur;
    [SerializeField] private TextMeshProUGUI teksDataRempah;
    [SerializeField] private TextMeshProUGUI teksDataDaging;
    [SerializeField] private TextMeshProUGUI teksDataKayu;
    [SerializeField] private TextMeshProUGUI teksDataBatu;
    [SerializeField] private TextMeshProUGUI teksDataTanahLiat;
    [SerializeField] private TextMeshProUGUI teksDataGems; 

    private void Update() {
        teksDataSayur.text = "x" + PersistentManager.Instance.dataStokSayur.ToString();
        teksDataRempah.text = "x" + PersistentManager.Instance.dataStokRempah.ToString();
        teksDataDaging.text = "x" + PersistentManager.Instance.dataStokDaging.ToString();

        teksDataKayu.text = "x" + PersistentManager.Instance.dataKayu.ToString();
        teksDataBatu.text = "x" + PersistentManager.Instance.dataBatu.ToString();
        teksDataTanahLiat.text = "x" + PersistentManager.Instance.dataTanahLiat.ToString();
        teksDataGems.text = "x" + PersistentManager.Instance.dataBatuAkik.ToString();
    }
}
