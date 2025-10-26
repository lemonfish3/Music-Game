using UnityEngine;

public class MapGameManager : MonoBehaviour
{
    public static MapGameManager Instance;
    public bool isPaused { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
