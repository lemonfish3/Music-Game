using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialogueNode", order = 1)]
public class DialogueNode : ScriptableObject
{
    [Header("Basic")]
    public string Speaker;
    public string[] Lines;

    [Header("Reply Options")]
    public ReplyOption[] ReplyOptions;
}

[System.Serializable]
public class ReplyOption
{
    [Header("Basic")]
    public string line;
    public DialogueNode nextNode;
}
