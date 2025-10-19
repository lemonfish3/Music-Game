using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; set; } // Singleton instance
    [Header("Audio Settings")]
    public AudioSource musicSource;
    public float BPM = 120f; // Beats per minute
    public float offsetTime = 0f; // Time offset in seconds
    
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
        }
        else
        {
            Destroy(gameObject);
        }

        beatInterval = 60f / BPM;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void StartMusic()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return;
        }

        songStartTime = (float)AudioSettings.dspTime;
        musicSource.Play();
        isPlaying = true;
    }
    void Start()
    {
        if (musicSource != null)
        {
            StartMusic();
        }

    }

    // Update is called once per frame
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

    public void StopMusic()
    {
        if (musicSource == null || !isPlaying)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return;
        }
        musicSource.Stop();
        isPlaying = false;
    }

    public void PauseMusic()
    {
        if (musicSource != null && isPlaying)
        {
            musicSource.Pause();
            isPlaying = false;
        }
    }

    public void ResumeMusic()
    {
        if (musicSource == null || isPlaying)
        {
            Debug.LogWarning("Music Source is not assigned.");
            return;
        }
        musicSource.UnPause();
        isPlaying = true;
    
    }

}
