using UnityEngine;
using System.Collections.Generic;

public class HitNoteManager : MonoBehaviour
{
    [Header("References")]
    public MusicManager musicManager;
    public KeyCode hitKey = KeyCode.Space;
    public float perfectThreshold = 0.1f;
    public float goodThreshold = 0.25f;
    public float missThreshold = 0.5f;

    [Header("Visuals")]
    public Color idleColor = Color.white;
    public Color activeColor = Color.yellow;
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;
    public float flashDuration = 0.2f;

    private SpriteRenderer sr;
    private float flashTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (musicManager == null)
        {
            musicManager = MusicManager.instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentBeat = musicManager.songPositionInBeats;
        
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                sr.color = idleColor;
            }
        }

        if (Input.GetKeyDown(hitKey))
        {
            sr.color = activeColor;
            EvaluateHit();
        }

        CheckMissedNotes(currentBeat);
    }

    void EvaluateHit()
    {



        float currentBeat = musicManager.songPositionInBeats;
        NoteMovement bestNote = null;
        float closetDiff = Mathf.Infinity;

        NoteMovement[] notes = FindObjectsOfType<NoteMovement>();

        foreach (var note in notes)
        {
            float diff = Mathf.Abs(note.targetBeat - currentBeat);
            if (diff < closetDiff)
            {
                closetDiff = diff;
                bestNote = note;
            }
        }

        if (bestNote == null)
        {
            return;
        }

        if (closetDiff <= perfectThreshold)
        {
            FlashColor(perfectColor);
            Debug.Log("Perfect Hit!");
            Destroy(bestNote.gameObject);
        }
        else if (closetDiff <= goodThreshold)
        {
            FlashColor(goodColor);
            Debug.Log("Good Hit!");
            Destroy(bestNote.gameObject);
        }
        else
        {
            FlashColor(missColor);
            Debug.Log("Miss!");
            Destroy(bestNote.gameObject);
        }
    }
    
    void CheckMissedNotes(float currentBeat)
    {
        NoteMovement[] notes = FindObjectsOfType<NoteMovement>();

        foreach (var note in notes)
        {
            // if currentBeat passed the note's target beat by more than the miss threshold
            if (currentBeat - note.targetBeat > missThreshold)
            {
                Debug.Log($"Missed note at beat {note.targetBeat}");
                FlashColor(missColor);
                Destroy(note.gameObject);
            }
        }
    }

    void FlashColor(Color color)
    {
        sr.color = color;
        flashTimer = flashDuration;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
