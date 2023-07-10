using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Construction")]
public class ConstructionSO : ScriptableObject
{
    public GameObject construction;
    public GameObject blueprint;

    public bool simpleConstruction = true;
    public bool verticalConstruction = true;
}
