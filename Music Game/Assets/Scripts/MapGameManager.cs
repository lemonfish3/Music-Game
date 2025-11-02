using UnityEngine;
using UnityEngine.UI;

public class MapGameManager : MonoBehaviour
{
    public static MapGameManager Instance;

    public SlideDisplay SlideDisplay;
    public DialogueNode SlidefirstNode;
    public AudioSource musicSource;
    public GameObject pauseMenuUI;
    [SerializeField] private Button pauseBack;


    public bool isPaused;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SlideDisplay.StartSlides(SlidefirstNode);
        pauseMenuUI.SetActive(false);
        pauseBack.onClick.AddListener(() => GameManager.instance?.BackToMain());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause is hit");
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        musicSource.Pause();
        pauseMenuUI.SetActive(true);
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        musicSource.UnPause();
        pauseMenuUI.SetActive(false);
    }
}
