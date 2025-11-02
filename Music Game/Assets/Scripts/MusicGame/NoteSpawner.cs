using UnityEngine;
using System.Collections.Generic;
public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    public NoteChart noteChart;
    public GameObject notePrefab;
    public Transform spawnPoint;
    public Transform targetPoint;
    public MusicManager musicManager;

    [Header("Settings")]
    public float noteSpawnLeadTime = 2.0f;
    private int nextNoteIndex = 0;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicManager == null)
        {
            Debug.LogWarning("MusicManager is not assigned.");
        }
        noteChart = musicManager.noteChart;
        Debug.Log($"Loaded NoteChart: {noteChart.songTitle} with {noteChart.notes.Count} notes.");
    }

    // Update is called once per frame
    void Update()
    {
        if (noteChart == null)
        {
            Debug.LogWarning("Music is not playing or NoteChart is not assigned.");
            return;
        }


        float songBeat = musicManager.songPositionInBeats;

        while (nextNoteIndex < noteChart.notes.Count &&
               noteChart.notes[nextNoteIndex].beat <= songBeat + (noteSpawnLeadTime))
        {
            SpawnNote(noteChart.notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(NoteData noteData)
    {
        if (notePrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("NotePrefab or SpawnPoint is not assigned.");
            return;
        }

        GameObject obj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);


        NoteMovement noteMovement = obj.GetComponent<NoteMovement>();
        if (noteMovement != null)
        {
            Debug.Log($"Configuring NoteMovement for note at beat {noteData.beat}");
            noteMovement.musicManager = musicManager;
            noteMovement.targetBeat = noteData.beat;
            noteMovement.startPosition = spawnPoint.position;
            noteMovement.targetPosition = targetPoint.position; // Example target position
            noteMovement.noteLeadBeats = noteSpawnLeadTime;
        }

        Debug.Log($"Spawned {noteData.noteType} note at beat {noteData.beat}, songBeat: {musicManager.songPositionInBeats:F2}");
    }

    public void ResetSpawner()
    {
        nextNoteIndex = 0;
        DestroyObjectsWithTag("Note");
        noteChart = MusicManager.instance.noteChart;
        if (noteChart == null)
        {
            Debug.LogWarning("NoteChart is not assigned.");
        }
        else
        {
            Debug.Log($"Loaded NoteChart: {noteChart.songTitle} with {noteChart.notes.Count} notes.");
        }
        if (musicManager == null)
        {
            Debug.LogWarning("MusicManager is not assigned.");
        }
        Debug.Log($"song beat{musicManager.songPositionInBeats}");
        Debug.Log($"[NoteSpawner] Resetting with chart: {noteChart.songTitle}");
        Debug.Log($"[NoteSpawner] Total notes: {noteChart.notes.Count}");

    }
    
    void DestroyObjectsWithTag(string tagToDestroy)
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tagToDestroy);

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }
}
