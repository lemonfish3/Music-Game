using UnityEngine;

[System.Serializable]
public class NoteData
{
    [Header("Timing")]
    public float beat;
    public float duration;

    [Header("Music Properties")]
    public NoteType noteType;
    public string noteName;
    public Sprite noteSprite;

    [Header("Gameplay Properties")]
    public int lane;
    public bool isLongNote;
    public bool isRestNote;

    public enum NoteType
    {
        Whole,
        Half,
        Quarter,
        Eighth,
        Sixteenth
    }
}

