using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelWindow : MonoBehaviour
{

    void Start() // called when set to activate
    {
        // PAUSE
        MapGameManager.Instance.PauseGame();
    }



    public void OnStartClick()
    {
        OpenMusicGame();
    }

    public void OnCancelClick()
    {
        gameObject.SetActive(false);

        //UNPAUSE
        MapGameManager.Instance.ResumeGame();
    }

    void OpenMusicGame()
    {
        SceneManager.LoadScene("Music");
    }

}
