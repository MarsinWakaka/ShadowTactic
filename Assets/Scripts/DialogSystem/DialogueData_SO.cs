using System.Collections.Generic;
using DialogSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialogueData> dialoguePieces = new();
}