using UnityEngine;

public class Respawner : MonoBehaviour {
    private void Start() {
        RespawnFurnitur();   
        RespawnSpesial();
    }
    
    private void RespawnFurnitur() {
        foreach (var furniturData in PersistentManager.Instance.dataFurniturList) {
            Instantiate(furniturData.furniturTypeSO.furniturPrefab, furniturData.furniturPosition, Quaternion.identity);
        }
        ScanAstar();
    }

    private void RespawnSpesial() {
        foreach (var spesialData in PersistentManager.Instance.dataSpesialList) {
            Instantiate(spesialData.spesialTypeSO.spesialPrefab, spesialData.spesialPosition, Quaternion.identity);
        }
        ScanAstar();
    }

    private void ScanAstar() {
        GameObject aStarObject = GameObject.Find("A_Star");

        if (aStarObject != null) {
            AstarPath pathfinder = aStarObject.GetComponent<AstarPath>(); // Menggunakan AstarPath
            if (pathfinder != null) {
                pathfinder.Scan(); // Panggil metode Scan
            } else {
                Debug.LogWarning("Komponen AstarPath tidak ditemukan pada GameObject A_Star!");
            }
        } else { 
            Debug.LogWarning("GameObject A_Star tidak ditemukan di scene!");
        }
    }

}
