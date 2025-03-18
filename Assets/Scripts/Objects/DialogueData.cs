using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public struct DialogueEntry
    {
        public string key;     
        [TextArea]            
        public string value;   
    }

    public DialogueEntry[] dialogueEntries;

    // internal dictionary
    private Dictionary<string, string> dialogueDictionary;

    // build dictionary if it isn't already
    private void InitDictionary()
    {
        if (dialogueDictionary != null) return;

        dialogueDictionary = new Dictionary<string, string>();
        foreach (var entry in dialogueEntries)
        {
            // add if not already present, 
            // just in case of accidental duplicate keys
            if (!dialogueDictionary.ContainsKey(entry.key))
            {
                dialogueDictionary.Add(entry.key, entry.value);
            }
            else
            {
                Debug.LogWarning($"Duplicate key found: {entry.key}. Skipping...");
            }
        }
    }

    // Public method to fetch dialogue text via 3-digit key
    public string GetDialogueByKey(string key)
    {
        InitDictionary();
        
        if (dialogueDictionary.TryGetValue(key, out string dialogueText))
        {
            return dialogueText;
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in dialogue data.");
            return string.Empty;
        }
    }
}