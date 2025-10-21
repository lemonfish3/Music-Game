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
    private bool isActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicManager == null)
        {
            musicManager = MusicManager.instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || musicManager == null) return;
        float currentBeat = musicManager.songPositionInBeats;
        float beatsUnitlTarget = targetBeat - currentBeat;
        Debug.Log($"Note at beat {targetBeat} | Current Beat: {currentBeat:F2} | Beats until target: {beatsUnitlTarget:F2}");

        if (beatsUnitlTarget <= 0)
        {
            // Note has reached or passed the target beat
            transform.position = targetPosition;
            isActive = false; // Stop updating position
            return;
        }

        float t = Mathf.InverseLerp(noteLeadBeats, 0, beatsUnitlTarget);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (beatsUnitlTarget <= -0.5f)
        {
            Destroy(gameObject);
            Debug.Log($"Note at beat {targetBeat} destroyed after passing target.");
        }
    }
    
}
