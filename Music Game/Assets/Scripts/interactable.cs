using NUnit.Framework.Interfaces;
using UnityEngine;

public class interactable : MonoBehaviour
{
    [SerializeField] private bool isPlayerNearby = false;

    //public GameObject highlight;

    //void Start()
    //{
    //    if (interactPrompt != null)
    //        interactPrompt.SetActive(false);
    //}

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("interact with the sculpture");
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // highlight.SetActive(true);

            //if (pickUpPrompt != null)
            //    pickUpPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;

            //highlight.SetActive(false);

            //if (pickUpPrompt != null)
            //    pickUpPrompt.SetActive(false);
        }
    }
}
