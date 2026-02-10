using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float displayDuration = 3f;

    private bool isShowingDialogue = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }
    }

    public void ShowDialogue(string text)
    {
        if (!isShowingDialogue)
        {
            StartCoroutine(DisplayDialogue(text));
        }
    }

    public void ShowMultipleLines(string[] lines, float pauseBetweenLines = 1.5f)
    {
        if (!isShowingDialogue)
        {
            StartCoroutine(DisplayMultipleLines(lines, pauseBetweenLines));
        }
    }

    IEnumerator DisplayDialogue(string text)
    {
        isShowingDialogue = true;

        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = "";

            foreach (char letter in text.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(displayDuration);

            dialogueText.gameObject.SetActive(false);
        }

        isShowingDialogue = false;
    }

    IEnumerator DisplayMultipleLines(string[] lines, float pauseBetweenLines)
    {
        isShowingDialogue = true;

        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);

            foreach (string line in lines)
            {
                dialogueText.text = "";

                foreach (char letter in line.ToCharArray())
                {
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }

                yield return new WaitForSeconds(pauseBetweenLines);
            }

            dialogueText.gameObject.SetActive(false);
        }

        isShowingDialogue = false;
    }
}