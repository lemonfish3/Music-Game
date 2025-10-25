using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;

    [Header("References")]
    public MusicManager musicManager;
    public HitNoteManager hitNoteManager;

    [Header("Game State")]
    public bool isGameActive = false;

    [Header("UI Elements")]
    public GameObject pauseMenuUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicManager == null)
        {
            musicManager = MusicManager.instance;
        }
        if (hitNoteManager == null)
        {
            hitNoteManager = HitNoteManager.instance;
        }
        musicManager.StartMusic();
        isGameActive = true;
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
        musicManager.PauseMusic();
        pauseMenuUI.SetActive(true);
        isGameActive = false;
    }

    public void Resume()
    {
        musicManager.ResumeMusic();
        pauseMenuUI.SetActive(false);
        isGameActive = true;
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Map");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Music");
    }
}

