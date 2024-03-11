using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "information", menuName = "InformationalData/InformationContent", order = 1)]
public class InformationalData : ScriptableObject
{
    public string title;
    [TextArea(3, 5)]
    public List<string> content;
    public List<Sprite> images;
}
