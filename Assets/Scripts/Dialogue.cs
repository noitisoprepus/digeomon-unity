using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogue", menuName = "DialogueData/DialogueObject", order = 1)]
public class Dialogue : ScriptableObject
{
    public string charName;
    public List<string> dialogues;
}
