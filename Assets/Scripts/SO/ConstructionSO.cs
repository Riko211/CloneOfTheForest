using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Construction")]
public class ConstructionSO : ScriptableObject
{
    public GameObject construction;
    public GameObject blueprint;

    public ConstructionType type;

    public bool verticalConstruction = true;

    public enum ConstructionType
    {
        SimpleConstruction,
        Foundation,
        Floor,
        Wall,
        Roof
    };
}
