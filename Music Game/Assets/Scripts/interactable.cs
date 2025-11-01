using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interactable : MonoBehaviour
{
    [SerializeField] private bool isPlayerNearby = false;
    public LevelWindow levelWindow;


    public GameObject interactPrompt;

    public NoteChart noteChart; // assign different note chart 

    void Start()
    {
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (MapGameManager.Instance.isPaused) return;

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("interact with the sculpture");
            levelWindow.SetMusicInfo(noteChart);
            levelWindow.gameObject.SetActive(true);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;

            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;

            interactPrompt.SetActive(false);
        }
    }
}
