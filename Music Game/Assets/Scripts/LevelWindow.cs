using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelWindow : MonoBehaviour
{

    public NoteChart music; // assign different note chart 

    [Header("UI")]
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI bestScore;

    void OnEnable() // called when set to activate
    {
        // PAUSE
        levelName.text = $"Level: {music.songTitle}";
        bestScore.text = $"best score: {music.highestScore}";
        MapGameManager.Instance.PauseGame();
    }

    

    public void OnStartClick()
    {
        OpenMusicGame();
        //UNPAUSE
        MapGameManager.Instance.ResumeGame();
        music.highestScore = GameManager.instance.noteChart.highestScore;
        Debug.Log($"back to map with {music.highestScore} points");
    }

    public void OnCancelClick()
    {
        gameObject.SetActive(false);

        //UNPAUSE
        MapGameManager.Instance.ResumeGame();
    }

    void OpenMusicGame()
    {
        GameManager.instance.StartGame(music);
    }

    public void SetMusicInfo(NoteChart noteChart)
    {
        music = noteChart;
        Debug.Log($"load notechart: {noteChart.songTitle}");
    }

}
