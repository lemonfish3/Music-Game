using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [SerializeField] private bool isPlayerNearby = false;
    public DialogueManager DialogueManager;
    public DialogueNode starterNode;


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
            DialogueManager.StartDialogue(starterNode);
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
