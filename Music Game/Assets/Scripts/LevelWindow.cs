using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelWindow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnStartClick()
    {
        OpenMusicGame();
    }

    public void OnCancelClick()
    {
        gameObject.SetActive(false);

        //UNPAUSE
        Time.timeScale = 1.0f;
    }

    void OpenMusicGame()
    {
        SceneManager.LoadScene("Music");
    }

}
