using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "digeomon", menuName = "DigeomonData/Digeomon", order = 1)]
public class DigeomonData : ScriptableObject
{
    [Header("Digeomon Info")]
    public string charName;
    [TextArea]
    public string description;
    public List<string> keys;
    public GameObject modelPrefab;
    public Sprite modelSprite;
    public DigeomonType type;

    [Header("Capturing Info")]
    public DialogueData introDialogue;
    public QuizData quiz;

    [Header("Evolution Info")]
    public DigeomonData evolution;
    public DigeomonData preEvolution;
}
