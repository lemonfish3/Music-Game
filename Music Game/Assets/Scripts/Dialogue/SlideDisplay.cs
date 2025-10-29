using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SlideDisplay : MonoBehaviour
{
    [Header("UI References")]
    public GameObject slidePanel;
    public TMP_Text slideText;
    public GameObject continueButton;

    [Header("Settings")]
    public float typingSpeed = 0.05f;

    private DialogueNode currentNode;
    private int lineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        slidePanel.SetActive(false);
        continueButton.SetActive(false);
    }

    public void StartSlides(DialogueNode firstNode)
    {
        currentNode = firstNode;
        lineIndex = 0;
        slidePanel.SetActive(true);
        ShowLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Finish typing immediately
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                slideText.text = currentNode.Lines[lineIndex];
                isTyping = false;
                //continueButton.SetActive(true);
            }
            else
            {
                // Move to next line or next node
                lineIndex++;
                if (lineIndex < currentNode.Lines.Length)
                {
                    ShowLine();
                }
                else
                {
                    // Move to next node via the single ReplyOption
                    if (currentNode.ReplyOptions.Length > 0 && currentNode.ReplyOptions[0].nextNode != null)
                    {
                        currentNode = currentNode.ReplyOptions[0].nextNode;
                        lineIndex = 0;
                        ShowLine();
                    }
                    else
                    {
                        // End of slides
                        continueButton.SetActive(true);
                        //slidePanel.SetActive(false);
                    }
                }
            }
        }
    }

    void ShowLine()
    {
        slideText.text = "";
        continueButton.SetActive(false);
        typingCoroutine = StartCoroutine(TypeLine(currentNode.Lines[lineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        foreach (char c in line)
        {
            slideText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        //continueButton.SetActive(true);
    }

    public void OnEnterButtonClick()
    {
        slidePanel.SetActive(false);
    }
}
