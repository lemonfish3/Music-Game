using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game Settings")]
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;

    // Accuracy tracking
    private int totalNotes = 0;
    private int hitNotes = 0; 

    [Header("References")]
    public MusicManager musicManager;
    public HitNoteManager hitNoteManager;
    public NoteChart noteChart;

    [Header("Game State")]
    public bool isGameActive = false;

    [Header("UI Elements")]
    public GameObject pauseMenuUI;
    public GameObject resultsPanel; // Assign your UI panel in Inspector
    public TMPro.TextMeshProUGUI finalScoreText;
    public TMPro.TextMeshProUGUI finalComboText;
    public TMPro.TextMeshProUGUI accuracyText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause is hit");
            if (isGameActive)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        if (SceneManager.GetActiveScene().name == "Music")
        {
            musicManager.PauseMusic();
            pauseMenuUI.SetActive(true);
            isGameActive = false;
        }

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (SceneManager.GetActiveScene().name == "Music")
        {
            musicManager.ResumeMusic();
            pauseMenuUI.SetActive(false);
            isGameActive = true;
        }
        Time.timeScale = 1f;
    }

    public void LoadMainGame()
    {
        QuitMusicGame();
        SceneManager.LoadScene("Map");
    }


    public void Restart()
    {
        resultsPanel.SetActive(false);
        pauseMenuUI.SetActive(false);
        score = 0;
        combo = 0;
        maxCombo = 0;
        hitNotes = 0;
        isGameActive = true;
        
        // Reset and restart the song
        musicManager.RestartMusic();

        // Clear all existing notes
        foreach (var note in FindObjectsOfType<NoteMovement>())
            Destroy(note.gameObject);

        // Reset spawner
        NoteSpawner spawner = FindObjectOfType<NoteSpawner>();
        if (spawner != null)
            spawner.ResetSpawner();
    }

    public void StartGame(NoteChart newChart)
    {
        noteChart = newChart;
        totalNotes = newChart.notes.Count;
        isGameActive = true;
        SceneManager.sceneLoaded += OnMusicSceneLoaded;
        SceneManager.LoadScene("Music");

        // Initialize gameplay systems here
        Debug.Log("Game started with new chart: " + newChart.name);
    }

    private void OnMusicSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Music")
        {
            // Find the MusicManager in the scene
            musicManager = MusicManager.instance ?? FindObjectOfType<MusicManager>();
            if (musicManager == null)
            {
                Debug.LogError("[GameManager] No MusicManager found in Music scene!");
                return;
            }

            musicManager.SetNoteChart(noteChart);
            musicManager.StartMusic();

            // Also setup HitNoteManager, NoteSpawner, etc.
            hitNoteManager = HitNoteManager.instance ?? FindObjectOfType<HitNoteManager>();

            SceneManager.sceneLoaded -= OnMusicSceneLoaded;
        }
    }

    public void RegisterHit(string rating)
    {
        int points = 0;

        switch (rating)
        {
            case "Perfect":
                points = 1000;
                combo++;
                hitNotes++;
                break;
            case "Good":
                points = 500;
                combo++;
                hitNotes++;
                break;
            case "Miss":
                combo = 0;
                break;
        }

        score += points;
        if (combo > maxCombo)
            maxCombo = combo;

        Debug.Log($"[GameManager] Score: {score} | Combo: {combo} | MaxCombo: {maxCombo}");
    }

    public void QuitMusicGame(){
        // Stop music and clean up
        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopMusic();
        }
        Debug.Log("Game quited!");
    }

    public void EndGame()
    {
        isGameActive = false;
        musicManager.PauseMusic();

        // Display results panel
        resultsPanel.SetActive(true);


        if (score > noteChart.highestScore)
        {
            noteChart.highestScore = score;
        }
        

        // Fill in results
        finalScoreText.text = $"Score: {score}";
        finalComboText.text = $"Max Combo: {maxCombo}";
        accuracyText.text = $"Accuracy: {GetAccuracy():F1}%";

        Debug.Log($"[GameManager] Game Ended | Score: {score} | Max Combo: {maxCombo} | Accuracy: {GetAccuracy():F1}%");
    }

    public float GetAccuracy()
    {
        if (totalNotes == 0) return 0f;
        float accuracy = (float)hitNotes / totalNotes * 100f;
        if (accuracy >= 0.6)
        {
            noteChart.passed = true;
        }
        return (float)hitNotes / totalNotes * 100f;
    }

    void SetVolume()
    {
        // change the volume for all scene?
        return;
    }
    
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
    
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Map");
    }

}

