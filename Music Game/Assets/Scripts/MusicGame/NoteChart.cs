using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NoteChart", menuName = "Scriptable Objects/NoteChart")]
public class NoteChart : ScriptableObject
{
    [Header("Song Info")]
    public string songTitle;
    public AudioClip musicClip;
    public float BPM = 120f;
    public float offset = 0f;

    [Header("Notes")]
    public List<NoteData> notes = new List<NoteData>();

    [Header("Lesson Info")]
    [TextArea] public string lessonDescription;
    public bool showNoteNames = true;

    public int highestScore = 0;

    public bool passed = false;
}
