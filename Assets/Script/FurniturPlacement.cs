using UnityEngine;
using UnityEngine.UI;

public class FurniturPlacement : MonoBehaviour {
    private FurniturManager furniturManager;
    private Vector3 placementPosition;

    private void Start() {
        
    }
    public void Setup(Vector3 position, FurniturManager manager) {
        placementPosition = position;
        furniturManager = manager;

        // Menemukan tombol dan menambahkan listener
        Button buttonAccept = transform.Find("Canvas/ButtonAccept").GetComponent<Button>();
        Button buttonCancel = transform.Find("Canvas/ButtonCancel").GetComponent<Button>();

        buttonAccept.onClick.AddListener(() => AcceptButtonPlacement());
        buttonCancel.onClick.AddListener(() => CancelButtonPlacement());

        Button buttonMerchant = GameObject.Find("ButtonMerchant").GetComponent<Button>(); 
        buttonMerchant.onClick.AddListener(() => {
            furniturManager.CancelPlacement();
            Destroy(gameObject);
        });
        
        Button buttonSpesial = GameObject.Find("ButtonSpesial").GetComponent<Button>(); 
        buttonSpesial.onClick.AddListener(() => {
            furniturManager.CancelPlacement();
            Destroy(gameObject);
        });
    }

    private void AcceptButtonPlacement() {
        // Panggil FurniturPlacing di FurniturManager
        furniturManager.FurniturPlacing(placementPosition);
        Destroy(gameObject); // Menghancurkan prefab placement setelah diterima
    }

    private void CancelButtonPlacement() {
        furniturManager.CancelPlacement(); // Reset status placement di FurniturManager
        Destroy(gameObject); // Menghancurkan prefab placement jika dibatalkan
    }
}
