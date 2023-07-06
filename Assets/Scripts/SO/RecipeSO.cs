using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Recipe")]
public class RecipeSO : ScriptableObject
{
    public ItemDataSO output;
    public int outputCount;

    public ItemDataSO[] input = new ItemDataSO[9]; 
}
