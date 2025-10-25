using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; } // Singleton instance
    [Header("Audio Settings")]
    public AudioSource musicSource;
    public float BPM = 120f; // Beats per minute
    public float offsetTime = 0f; // Time offset in seconds

    [Header("Note Chart")]
    public NoteChart noteChart;

    
    [Header("Debug Info")]
    public bool isPlaying = false;
    public float songPosition;
    public float songPositionInBeats;

    private float beatInterval;
    private float songStartTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                if (musicSource == null)
                {
                    Debug.LogError("Failed to create AudioSource component.");
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        NoteChart selectedChart = noteChart; // Load from somewhere (ScriptableObject, list, etc.)
        MusicManager.instance.SetNoteChart(selectedChart);
        MusicManager.instance.StartMusic();
    }


    void Update()
    {
        if (!isPlaying) return;
        songPosition = (float)(AudioSettings.dspTime - songStartTime) + offsetTime;
        songPositionInBeats = songPosition / beatInterval;
    }

    public float GetSongTimeSeconds()
    {
        if (musicSource == null || !isPlaying)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return 0f;
        }
        return musicSource.time + offsetTime;

    }

    public float GetSongTimeBeats()
    {
        return GetSongTimeSeconds() / beatInterval;
    }

    public void SetNoteChart(NoteChart newChart)
    {
        noteChart = newChart;
        if (noteChart != null)
        {
            musicSource.clip = noteChart.musicClip;
            BPM = noteChart.BPM;
            offsetTime = noteChart.offset;
            beatInterval = 60f / BPM;
        }
    }

    public void StartMusic()
    {
        if (noteChart == null)
        {
            Debug.LogWarning("NoteChart is not assigned.");
            return;
        }
        if (musicSource.clip == null)
        {
            musicSource.clip = noteChart.musicClip;
        }
        if (musicSource.clip == null)
        {
            Debug.LogWarning("No AudioClip assigned to NoteChart!");
            return;
        }

        StartCoroutine(DelayedMusicStart(offsetTime));
    }
    IEnumerator DelayedMusicStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        musicSource.Play();
        songStartTime = (float)AudioSettings.dspTime;
        isPlaying = true;
    }



    public void StopMusic()
    {
        if (!isPlaying)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return;
        }
        musicSource.Stop();
        isPlaying = false;
    }

    public void PauseMusic()
    {
        if (isPlaying)
        {
            musicSource.Pause();
            isPlaying = false;
        }
    }

    public void ResumeMusic()
    {
        if (isPlaying)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return;
        }
        musicSource.UnPause();
        isPlaying = true;
    
    }

}
