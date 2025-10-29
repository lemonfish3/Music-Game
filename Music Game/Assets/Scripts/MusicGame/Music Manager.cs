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
    private float pauseStartDspTime = 0f;

    [Header("Reference")]
    private GameManager GameManagerInstance;

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
        ResetMusicSettings();
        NoteChart selectedChart = noteChart; // Load from somewhere (ScriptableObject, list, etc.)
        if (noteChart == null)
        {
            Debug.LogError("No NoteChart assigned! Please assign one in the Inspector or load dynamically.");
            return;
        }
        MusicManager.instance.SetNoteChart(selectedChart);
        MusicManager.instance.StartMusic();
        GameManagerInstance = FindObjectOfType<GameManager>();
    }

    // void Start()
    // {
    //     NoteChart selectedChart = noteChart; // Load from somewhere (ScriptableObject, list, etc.)
    //     if (noteChart == null)
    //     {
    //         Debug.LogError("No NoteChart assigned! Please assign one in the Inspector or load dynamically.");
    //         return;
    //     }
    //     MusicManager.instance.SetNoteChart(selectedChart);
    //     MusicManager.instance.StartMusic();
    //     GameManagerInstance = FindObjectOfType<GameManager>();
    // }

    public void ResetMusicSettings(bool keepChart = true)
    {
        StopAllCoroutines();

        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.time = 0f;
        }

        isPlaying = false;
        songPosition = 0f;
        songPositionInBeats = 0f;
        songStartTime = 0f;
        pauseStartDspTime = 0f;

        // clear chart if specified
        if (!keepChart)
        {
            noteChart = null;
            musicSource.clip = null;
        }

        Debug.Log("[MusicManager] Music system reset.");
    }

    public void RestartMusic()
    {
        ResetMusicSettings(true); // Reset but keep current note chart
        StartMusic();
    }

    void Update()
    {
        if (!isPlaying) return;
        songPosition = (float)(AudioSettings.dspTime - songStartTime) + offsetTime;
        songPositionInBeats = songPosition / beatInterval;

        if (songPosition >= musicSource.clip.length)
        {
            GameManager.instance.EndGame();
        }
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
            // record when we paused using DSP time so we can exclude the paused duration from song timing
            pauseStartDspTime = (float)AudioSettings.dspTime;
            musicSource.Pause();
            isPlaying = false;
            // Do not modify Time.timeScale here — pausing the audio/beat timer should not freeze the whole engine
        }
    }

    public void ResumeMusic()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return;
        }
        if (isPlaying)
        {
            // already playing
            return;
        }

        // Adjust songStartTime so the paused duration is not counted in the song position
        float pauseEndDsp = (float)AudioSettings.dspTime;
        float pausedDuration = pauseEndDsp - pauseStartDspTime;
        songStartTime += pausedDuration;

        musicSource.UnPause();
        isPlaying = true;

    
    }

}
