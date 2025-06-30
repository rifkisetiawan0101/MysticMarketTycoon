using UnityEngine;

[CreateAssetMenu(fileName = "Spesial", menuName = "Scriptable Object/Spesial")]
public class SpesialTypeSO : ScriptableObject {
    public string spesialName;
    public float spesialPrice;
    public Transform spesialPrefab;
    public Transform spesialConstructionPrefab;
    public Sprite spesialButton;
    public Sprite selectedSpesialButton;
    public Sprite spesialWindow;
    public GameObject spesialCursor;
    public Transform spesialPlacementPrefab;
}
