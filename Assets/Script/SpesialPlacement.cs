using UnityEngine;
using UnityEngine.UI;

public class SpesialPlacement : MonoBehaviour {
    private SpesialManager spesialManager;
    private Vector3 placementPosition;
    private SpesialTypeSO spesialTypeSO;

    private void Start() {
        
    }
    public void Setup(Vector3 position, SpesialManager manager) {
        placementPosition = position;
        spesialManager = manager;

        // Menemukan tombol dan menambahkan listener
        Button buttonAccept = transform.Find("Canvas/ButtonAccept").GetComponent<Button>();
        Button buttonCancel = transform.Find("Canvas/ButtonCancel").GetComponent<Button>();

        buttonAccept.onClick.AddListener(() => AcceptButtonPlacement());
        buttonCancel.onClick.AddListener(() => CancelButtonPlacement());

        Button buttonMerchant = GameObject.Find("ButtonMerchant").GetComponent<Button>(); 
        buttonMerchant.onClick.AddListener(() => {
            spesialManager.CancelPlacement();
            Destroy(gameObject);
        });

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            spesialManager.CancelPlacement();
            Destroy(gameObject);
        });
    }

    private void AcceptButtonPlacement() {
        // Panggil SpesialPlacing di SpesialManager
        spesialManager.SpesialPlacing(placementPosition);
        Destroy(gameObject); // Menghancurkan prefab placement setelah diterima
    }

    private void CancelButtonPlacement() {
        spesialManager.CancelPlacement(); // Reset status placement di SpesialManager
        Destroy(gameObject); // Menghancurkan prefab placement jika dibatalkan
    }
}
