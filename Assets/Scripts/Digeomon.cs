using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "digeomon", menuName = "DigeomonData/Digeomon", order = 1)]
public class Digeomon : ScriptableObject
{
    [Header("Digeomon Info")]
    public string charName;
    public List<string> keys;
    public GameObject modelPrefab;
    public Sprite modelSprite;

    [Header("Capturing Info")]
    public Dialogue introDialogue;
    public Quiz quiz;
    // TODO: Add other info
}
