using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    [Header("Reply UI")]
    public Transform replyButtonParent;
    public GameObject replyButtonPrefab;

    [Header("System")]
    [SerializeField] bool dialogue_active;
    private DialogueNode current_node;
    private int line_index = 0;


    private void Start()
    {
        dialoguePanel.SetActive(false);
        replyButtonParent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!dialogue_active)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueNode node)
    {
        // basic set
        dialogue_active = true;
        current_node = node;
        line_index = 0;
        dialoguePanel.SetActive(true);
        
        speakerText.text = node.Speaker;

        // start line
        NextLine();
    }



    private void NextLine()
    {
        if (line_index < current_node.Lines.Length)
        {
            // set
            dialogueText.text = current_node.Lines[line_index];
            line_index++;
        }
        else
        {
            if (current_node.ReplyOptions != null && current_node.ReplyOptions.Length > 0)
            {
                ShowReplyOptions();
            }
            else
            {
                EndDialogue();
            }
        }
    }



    private void ShowReplyOptions()
    {
        replyButtonParent.gameObject.SetActive(true);

        foreach (Transform child in replyButtonParent) // clear reply options
            Destroy(child.gameObject);

        foreach (var reply in current_node.ReplyOptions)
        {
            GameObject buttonObj = Instantiate(replyButtonPrefab, replyButtonParent);
            buttonObj.GetComponentInChildren<TMP_Text>().text = reply.line;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnClickReply(reply));
        }
    }


    private void OnClickReply(ReplyOption reply)
    {
        foreach (Transform child in replyButtonParent) // clear options
            Destroy(child.gameObject);

        replyButtonParent.gameObject.SetActive(false);
        StartDialogue(reply.nextNode);
    }



    private void EndDialogue()
    {
        dialogue_active = false;
        dialoguePanel.SetActive(false);
        current_node = null;

        //RESUME
        Time.timeScale = 1.0f;
    }

}

