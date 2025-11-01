using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool isPlayerInRange = false;

    private void Update()
    {
        HandleDialogueInput();
    }

    private void HandleDialogueInput()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Z) && !DialogueManager.GetInstance().IsDialoguePlaying())
        {
            DialogueManager.GetInstance().EnterDialogue(inkJSON);
            Debug.Log("Dialogue started from trigger.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
