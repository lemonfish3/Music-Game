using UnityEngine;
using System.Collections.Generic;

public class HitNoteManager : MonoBehaviour
{
    public static HitNoteManager instance;

    [Header("References")]
    public MusicManager musicManager;
    public KeyCode hitKey = KeyCode.Space;
    private GameManager GameManagerInstance;

    [Header("Hit Timing (in beats)")]
    public float perfectThreshold = 0.1f;
    public float goodThreshold = 0.25f;
    public float missThreshold = 0.5f;

    [Header("UI Feedback")]
    public string currentRating = ""; // Used by RatingUI
    public string defaultRating = ""; // Rating to show when no judgment is active
    public float resetRatingTime = 0.2f; // How long to show the rating before resetting
    private float ratingResetTimer = 0f; // Timer for auto-resetting the rating

    private HashSet<NoteMovement> judgedNotes = new HashSet<NoteMovement>();

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
        GameManagerInstance = FindObjectOfType<GameManager>();
    }

    private NoteMovement[] activeNotes = new NoteMovement[0];
    private float nextNoteSearch = 0f;
    private const float NOTE_SEARCH_INTERVAL = 0.1f; // How often to refresh note list

    void Update()
    {
        if (musicManager == null) return;

        // If music is not playing (paused/stopped), skip hit/miss processing
        if (!musicManager.isPlaying) return;

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

        // Check if we should reset the rating
        if (ratingResetTimer > 0)
        {
            ratingResetTimer -= Time.deltaTime;
            if (ratingResetTimer <= 0)
            {
                currentRating = defaultRating;
                Debug.Log($"[HitNoteManager] Rating reset to default: {defaultRating}");
            }
        }
    }

    void EvaluateHit()
    {
        float currentBeat = musicManager.songPositionInBeats;
        NoteMovement bestNote = null;
        float closestDiff = Mathf.Infinity;

        foreach (var note in activeNotes)
        {
            if (note == null) continue;
            if (note.isJudged) continue;                     // respect note flag
            if (judgedNotes.Contains(note)) continue;       // defensive check

            float diff = Mathf.Abs(note.targetBeat - currentBeat);
            if (diff < closestDiff)
            {
                closestDiff = diff;
                bestNote = note;
            }
        }


        // If we found a candidate, only mark it judged if it's within the missThreshold.
        if (bestNote != null && closestDiff <= missThreshold)
        {
            // Now that we know it's a valid candidate, mark judged and apply rating
            bestNote.isJudged = true;
            judgedNotes.Add(bestNote);

            if (closestDiff <= perfectThreshold)
            {
                SetRating("Perfect");
            }
            else if (closestDiff <= goodThreshold)
            {
                SetRating("Good");
            }
            else // between good and missThreshold but still within missThreshold -> Miss
            {
                Debug.Log("pressed");
                // This happens if the player pressed within missThreshold but outside good/perfect.
                SetRating("Miss");
            }
        }
        else
        {
            // no valid note within timing window
            Debug.Log("No valid note in timing window.");
        }
    }

    void CheckMissedNotes(float currentBeat)
    {
        foreach (var note in activeNotes)
        {
            if (note == null) continue;
            if (note.isJudged) continue;
            if (judgedNotes.Contains(note)) continue;

            // Only consider a miss if the beat has passed (we are past the note's target)
            float timeSince = currentBeat - note.targetBeat; // positive if we passed the beat
            if (timeSince >= missThreshold)
            {
                // mark judged as missed
                note.isJudged = true;
                judgedNotes.Add(note);

                SetRating("Miss");
                Debug.Log($"[HitNoteManager] Miss detected for note {note.targetBeat:F3} | currentBeat {currentBeat:F3} | diff {timeSince:F3}");
            }
        }
    }

    void SetRating(string rating)
    {
        currentRating = rating;
        ratingResetTimer = resetRatingTime;
        Debug.Log($"[{Time.time:F2}] {rating}!");

        // Update score via GameManager
        if (GameManagerInstance != null)
            GameManagerInstance.RegisterHit(rating);
    }

    
}
