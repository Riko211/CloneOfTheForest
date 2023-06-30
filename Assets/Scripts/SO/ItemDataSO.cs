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

    [Header("Dropped item")]
    public GameObject prefab;


    public enum ItemType
    {
        Tool,
        Material
    };
}
