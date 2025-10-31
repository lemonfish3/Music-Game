using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelWindow : MonoBehaviour
{
    public NoteChart noteChart; // assign different note chart 
    private int highestScore;
    
    void OnEnable() // called when set to activate
    {
        // PAUSE
        MapGameManager.Instance.PauseGame();
    }

    

    public void OnStartClick()
    {
        OpenMusicGame();
        //UNPAUSE
        MapGameManager.Instance.ResumeGame();
        highestScore = GameManager.instance.noteChart.highestScore;
        Debug.Log($"back to map with {highestScore} points");
    }

    public void OnCancelClick()
    {
        gameObject.SetActive(false);

        //UNPAUSE
        MapGameManager.Instance.ResumeGame();
    }

    void OpenMusicGame()
    {
        GameManager.instance.StartGame(noteChart);
        // SceneManager.LoadScene("Music");
    }

}
