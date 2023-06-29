using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemDataSO : ScriptableObject
{
    [Header("Gameplay data")]

    [Header("UI data")]
    public bool stackable = true;
    public int maxStackSize = 1;

    [Header("Other")]
    public Sprite image;
}
