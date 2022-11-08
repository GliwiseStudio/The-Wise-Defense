using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager _dialogueManager;

    [TextArea(1, 10)]
    public string[] sentences;

    public void TriggerDialogue(LevelSelection levelSelection)
    {
        Debug.Log(_dialogueManager);
        _dialogueManager.gameObject.SetActive(true);
        _dialogueManager.StartDialogue(sentences, levelSelection);
    }
}
