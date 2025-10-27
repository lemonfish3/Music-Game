using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    [Header("References")]
    public MusicManager musicManager;

    [Header("Settings")]
    public float targetBeat; // The beat at which the note should reach the target position
    public Vector3 targetPosition;
    public Vector3 startPosition;
    public float noteLeadBeats = 2f; // How many beats before the target beat the note spawns
    [Header("Timing")]
    public float lingerDuration = 2f;
    private bool isActive = true;
    private Vector3 endPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool isJudged = false;
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
        float spawnBeat = targetBeat - noteLeadBeats;

        float t = (currentBeat - spawnBeat) / (noteLeadBeats + lingerDuration);
        t = Mathf.Clamp01(t); 
        // Debug.Log($"Note at beat {targetBeat} | Current Beat: {currentBeat:F2} | Beats until target: {beatsUnitlTarget:F2}");
        Vector3 finalPosition = targetPosition + (targetPosition - startPosition).normalized * lingerDuration;
        transform.position = Vector3.Lerp(startPosition, finalPosition, t);

        // Destroy after linger
        if (currentBeat >= targetBeat + lingerDuration)
        {
            Destroy(gameObject);
        }
    }
    
}
