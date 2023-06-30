using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemDataSO : ScriptableObject
{
    [Header("Inventory")]
    public bool stackable = true;
    public int maxStackSize = 1;
    public ItemType type;

    [Header("UI")]
    public Sprite image;

    [Header("Collectable item")]
    public GameObject collectablePrefab;

    [Header("InHand item (for tools)")]
    public GameObject inHandPrefab;


    public enum ItemType
    {
        Tool,
        Material
    };
}
