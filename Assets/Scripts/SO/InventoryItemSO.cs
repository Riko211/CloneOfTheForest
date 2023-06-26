using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class InventoryItemSO : ScriptableObject
{
    [Header("Gameplay data")]

    [Header("UI data")]
    public bool stackable = true;

    [Header("Other")]
    public Sprite image;
}
