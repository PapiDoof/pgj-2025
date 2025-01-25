using UnityEngine;
using System.Collections;
using TMPro;
using System.Diagnostics;

public class ChatBubble : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText; // Chat bubble text object
    public string[] dialogue; // Regular dialogue
    public string[] choices; // Choices text
    private int index = 0;

    public float wordSpeed = 0.05f;
    public bool playerIsClose;
    private bool isChoiceMode = false; // Are we in choice selection mode?
    private int selectedChoice = 0; // Tracks the currently selected choice

    void Start()
    {
        GameObject rootCanvas = dialoguePanel.transform.root.gameObject;
        DontDestroyOnLoad(rootCanvas);
        if (dialoguePanel == null)
        {
            UnityEngine.Debug.LogError("Dialogue panel is not assigned in the Inspector! Please assign it.");
        }
        dialogueText.text = "";
    }

    void Update()
    {
        if (dialoguePanel == null)
        {
            UnityEngine.Debug.LogError("Dialogue panel became null during gameplay! Check if it's being destroyed.");
        }
        // Start dialogue or progress through dialogue
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && !isChoiceMode)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                if (index == dialogue.Length - 1) // When reaching the last dialogue
                {
                    ShowChoices();
                }
                else
                {
                    NextLine();
                }
            }
        }

        // Exit dialogue
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }

        // Handle choices when in choice mode
        if (isChoiceMode)
        {
            if (Input.GetKeyDown(KeyCode.W)) // Move up
            {
                selectedChoice = (selectedChoice - 1 + choices.Length) % choices.Length;
                DisplayChoices();
            }
            else if (Input.GetKeyDown(KeyCode.S)) // Move down
            {
                selectedChoice = (selectedChoice + 1) % choices.Length;
                DisplayChoices();
            }
            else if (Input.GetKeyDown(KeyCode.F)) // Confirm choice
            {
                HandleChoice(selectedChoice);
            }
        }
    }

    public void RemoveText()
    {
        if (dialoguePanel == null)
        {
            UnityEngine.Debug.LogError("Dialogue panel is null in RemoveText! Check if it has been destroyed or not assigned.");
            return;
        }

        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        isChoiceMode = false;
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ShowChoices();
        }
    }

    // Displays the choices after the dialogue
    public void ShowChoices()
    {
        UnityEngine.Debug.Log("Entering choice mode...");
        isChoiceMode = true; // Enter choice mode
        selectedChoice = 0; // Reset choice selection
        DisplayChoices();
    }

    // Updates the choice display
    public void DisplayChoices()
    {
        string fullText = dialogue[index] + "\n\n"; // Keep the last dialogue line at the top
        for (int i = 0; i < choices.Length; i++)
        {
            UnityEngine.Debug.Log("Selecting choice mode...");
            if (i == selectedChoice)
            {
                fullText += $">> {choices[i]} <<\n"; // Highlight the selected choice
            }
            else
            {
                fullText += $"{choices[i]}\n"; // Regular choice text
            }
        }
        dialogueText.text = fullText;
    }

    // Handles the selected choice
    public void HandleChoice(int choiceIndex)
    {
        isChoiceMode = false;
        if (choiceIndex == 0)
        {
            // Continue the game or dialogue
            dialogueText.text = "Great! Let's continue!";
        }
        else if (choiceIndex == 1)
        {
            // Exit or close the dialogue
            RemoveText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
            if (other.CompareTag("Player"))
            {
                playerIsClose = false;

                if (dialoguePanel == null)
                {
                    UnityEngine.Debug.LogError("Dialogue panel is null in OnTriggerExit2D! This should not happen.");
                    return;
                }

                RemoveText();
            }
    }
}
