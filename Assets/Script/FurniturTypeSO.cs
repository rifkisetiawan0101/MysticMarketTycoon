using UnityEngine;

[CreateAssetMenu(fileName = "Furnitur", menuName = "Scriptable Object/Furnitur")]
public class FurniturTypeSO : ScriptableObject {
    public string furniturName;
    public float furniturPrice;
    public Transform furniturPrefab;
    public Transform furniturConstructionPrefab;
    public Sprite furniturButton;
    public Sprite selectedFurniturButton;
    public Sprite furniturWindow;
    public GameObject furniturCursor;
    public Transform furniturPlacementPrefab;
}
