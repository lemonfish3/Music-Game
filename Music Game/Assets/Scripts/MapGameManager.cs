using UnityEngine;

public class MapGameManager : MonoBehaviour
{
    public static MapGameManager Instance;

    public SlideDisplay SlideDisplay;
    public DialogueNode SlidefirstNode;


    public bool isPaused;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SlideDisplay.StartSlides(SlidefirstNode);
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
}
