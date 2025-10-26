using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interactable : MonoBehaviour
{
    [SerializeField] private bool isPlayerNearby = false;
    public GameObject LevelWindow;


    public GameObject interactPrompt;

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
            LevelWindow.SetActive(true);
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
