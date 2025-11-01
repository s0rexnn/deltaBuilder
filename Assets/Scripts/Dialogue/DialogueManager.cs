using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Adjustments")]
    [SerializeField] private float typeSpeed = 5f;  

    private Story currentStory;
    private bool dialogueIsPlaying = false;
    private Coroutine typingCoroutine;
    private bool canContinueToNextLine = false;
    public static DialogueManager instance;

    private const string HTML_ALPHA = "<alpha=#00>"; 
    private const float MAX_TYPE_TIME = 0.05f;        

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instances of DialogueManager detected.");

        instance = this;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;
        HandleDialogueInput();

        if (dialogueIsPlaying)
            EventsHandler.Instance.DisableMovement();
        else
            EventsHandler.Instance.EnableMovement();
    }

    public static DialogueManager GetInstance() => instance;
    public bool IsDialoguePlaying() => dialogueIsPlaying;

    public void EnterDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueDialogue();
    }

    private void ExitDialogue()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentStory = null;
    }

    private void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            string line = currentStory.Continue();
            typingCoroutine = StartCoroutine(TypeDialogueText(line));
        }
        else
        {
            ExitDialogue();
        }
    }

    private IEnumerator TypeDialogueText(string line)
    {
        canContinueToNextLine = false;

        // Store the full original text
        string originalText = line;
        string displayedText;
        int alphaIndex = 0;


        dialogueText.text = originalText;

        // Reveal text using alpha tags
        while (alphaIndex < originalText.Length)
        {
            alphaIndex++;

            displayedText = originalText.Insert(alphaIndex, HTML_ALPHA);
            dialogueText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        // Show everything at the end
        dialogueText.text = originalText;
        canContinueToNextLine = true;
    }

    private void HandleDialogueInput()
    {

        if (Input.GetKeyDown(KeyCode.Z) && dialogueIsPlaying && canContinueToNextLine)
        {
            ContinueDialogue();
        }

        else if (Input.GetKeyDown(KeyCode.X) && dialogueIsPlaying && !canContinueToNextLine)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = currentStory.currentText;
            canContinueToNextLine = true;
        }
    }
}
