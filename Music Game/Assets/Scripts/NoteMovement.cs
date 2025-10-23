using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    [Header("References")]
    public MusicManager musicManager;

    [Header("Settings")]
    public float targetBeat; // The beat at which the note should reach the target position
    public Vector3 targetPosition;
    public Vector3 startPosition;
    public float noteLeadBeats = 2.0f; // How many beats before the target beat the note spawns
    [Header("Timing")]
    public float lingerDuration = 0.5f;
    private bool isActive = true;
    private Vector3 endPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicManager == null)
        {
            musicManager = MusicManager.instance;
        }
        Vector3 direction = (targetPosition - startPosition).normalized;
        endPosition = targetPosition + direction * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (musicManager == null) return;
        float currentBeat = musicManager.songPositionInBeats;
        float beatsUnitlTarget = targetBeat - currentBeat;
        // Debug.Log($"Note at beat {targetBeat} | Current Beat: {currentBeat:F2} | Beats until target: {beatsUnitlTarget:F2}");
        float totalBeats = noteLeadBeats + lingerDuration; // Including extra distance after target
        float t = Mathf.InverseLerp(noteLeadBeats, -lingerDuration, beatsUnitlTarget);
        transform.position = Vector3.Lerp(startPosition, endPosition, t);

        if (beatsUnitlTarget <= -lingerDuration)
        {
            isActive = false;
            Debug.Log($"Note at beat {targetBeat} destroyed at {currentBeat} after reaching target.");
            Destroy(gameObject);
        }
    }
    
}
