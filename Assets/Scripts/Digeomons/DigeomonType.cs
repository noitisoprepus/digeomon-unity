using UnityEngine;

[CreateAssetMenu(fileName = "digeomonType", menuName = "DigeomonData/Digeomon Type", order = 1)]
public class DigeomonType: ScriptableObject
{
    public string name;
    public string description;

    [Header("2D")]
    public string shape2D;
    public string area;
    public string perimeter;

    [Header("3D")]
    public string shape3D;
    public string volume;
    public string surfaceArea;
}
