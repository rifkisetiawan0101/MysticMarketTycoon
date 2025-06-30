using UnityEngine;

[CreateAssetMenu(fileName = "Merchants", menuName = "Scriptable Object/Merchants")]
public class MerchantTypeSO : ScriptableObject {
    public string merchantName;
    public float merchantPrice;
    public Transform merchantPrefab;
    public Transform merchantConstructionPrefab;
    public Sprite merchantButton;
    public Sprite selectedMerchantButton;
    public Sprite merchantWindow;
    public GameObject merchantCursor;
    public Transform merchantPlacementPrefab;
}
