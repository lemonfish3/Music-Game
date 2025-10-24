using UnityEngine;

public class HitNoteManager : MonoBehaviour
{
    public static HitNoteManager instance;

    [Header("References")]
    public MusicManager musicManager;
    public KeyCode hitKey = KeyCode.Space;

    [Header("Hit Timing (in beats)")]
    public float perfectThreshold = 0.1f;
    public float goodThreshold = 0.25f;
    public float missThreshold = 0.5f;

    [Header("UI Feedback")]
    public string currentRating = ""; // Used by RatingUI

    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        if (musicManager == null)
        {
            musicManager = MusicManager.instance;
        }
    }

    private NoteMovement[] activeNotes = new NoteMovement[0];
    private float nextNoteSearch = 0f;
    private const float NOTE_SEARCH_INTERVAL = 0.1f; // How often to refresh note list

    void Update()
    {
        if (musicManager == null) return;

        float currentBeat = musicManager.songPositionInBeats;

        // Refresh note list periodically instead of every frame
        if (Time.time >= nextNoteSearch)
        {
            activeNotes = FindObjectsOfType<NoteMovement>();
            nextNoteSearch = Time.time + NOTE_SEARCH_INTERVAL;
        }

        if (Input.GetKeyDown(hitKey))
        {
            EvaluateHit();
        }

        CheckMissedNotes(currentBeat);
    }

    void EvaluateHit()
    {
        float currentBeat = musicManager.songPositionInBeats;
        NoteMovement bestNote = null;
        float closestDiff = Mathf.Infinity;

        foreach (var note in activeNotes)
        {
            if (note == null || note.isJudged) continue;

            float diff = Mathf.Abs(note.targetBeat - currentBeat);
            if (diff < closestDiff)
            {
                closestDiff = diff;
                bestNote = note;
            }
        }

        if (bestNote == null)
        {
            Debug.Log("No note to hit!");
            return;
        }

        // Judge accuracy
        if (closestDiff <= perfectThreshold)
        {
            SetRating("Perfect");
        }
        else if (closestDiff <= goodThreshold)
        {
            SetRating("Good");
        }
        else if (closestDiff <= missThreshold)
        {
            SetRating("Miss");
        }
        else
        {
            // If player hits way too early or late, ignore
            Debug.Log("No valid note in timing window.");
        }
    }

    void CheckMissedNotes(float currentBeat)
    {
        foreach (var note in activeNotes)
        {
            if (note == null || note.isJudged) continue;

            float diff = currentBeat - note.targetBeat; // positive if we passed the beat
            if (diff >= missThreshold)
            {
                note.isJudged = true;
                SetRating("Miss");
                Debug.Log($"[HitNoteManager] Miss detected for note {note.targetBeat:F3} | currentBeat {currentBeat:F3} | diff {diff:F3}");
                // Let NoteMovement handle the natural destruction
            }
        }
    }

    void SetRating(string rating)
    {
        currentRating = rating;
        Debug.Log($"[{Time.time:F2}] {rating}!");
    }
}
