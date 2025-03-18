using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText;  
    [SerializeField] private GameObject dialogueBox;         

    [Header("Dialogue Data")]
    [SerializeField] private DialogueData dialogueData;      

    
    [SerializeField] private string[] keyOrder = { "001", "002", "003" };
    private int currentIndex = 0;

    private void Start()
    {
        // Intention will probably be to hide the dialogue box on start
        dialogueBox.SetActive(false);

        // For testing, have text immediately pop up
        StartDialogueSequence();
    }

    // Display text for a specific key
    public void DisplayDialogue(string key)
    {
        string textToDisplay = dialogueData.GetDialogueByKey(key);
        dialogueBox.SetActive(true);
        dialogueText.text = textToDisplay;
    }

    // Method to turn the text box on or off
    public void ToggleDialogueBox(bool show)
    {
        dialogueBox.SetActive(show);
    }

    // Input method to advance text using the keyOrder array
    private void Update()
    {
        // If dialogue is active, press Space to advance
        if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex++;
            if (currentIndex < keyOrder.Length)
            {
                DisplayDialogue(keyOrder[currentIndex]);
            }
            else
            {
                // End of the ordered dialogue sequence
                dialogueBox.SetActive(false);
            }
        }
    }

    // Optional function to start the order of dialogues at the beginning
    public void StartDialogueSequence()
    {
        if (keyOrder.Length > 0)
        {
            currentIndex = 0;
            DisplayDialogue(keyOrder[currentIndex]);
        }
    }
}