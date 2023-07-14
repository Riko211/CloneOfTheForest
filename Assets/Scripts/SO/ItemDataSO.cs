using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemDataSO : ScriptableObject
{
    [Header("Inventory")]
    public bool stackable = true;
    public int maxStackSize = 1;
    public ItemType type;

    [Space, Header("UI, sprite for inventory item")]
    public Sprite image;

    [Space, Header("Collectable item(dropped on ground item prefab)")]
    public GameObject collectablePrefab;

    [Space, Header("InHand item (prefab for tools and construction blueprints)")]
    public GameObject inHandPrefab;

    [Space, Header("Prefabs for constructions")]
    public GameObject construction;
    public GameObject blueprint;

    public ConstructionType constructionType;

    public bool verticalConstruction = true;




    public enum ItemType
    {
        Tool,
        Material
    };
    public enum ConstructionType
    {
        SimpleConstruction,
        Foundation,
        Floor,
        Wall,
        Roof
    };
}
