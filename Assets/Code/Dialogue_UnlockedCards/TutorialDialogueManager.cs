using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueManager : MonoBehaviour
{
    private Queue<string> _sentences;
    [SerializeField] private TextMeshProUGUI _text;
    private Queue<TutorialDialogueTrigger> _dialogues;

    void Awake()
    {
        _dialogues = new Queue<TutorialDialogueTrigger>();
        TutorialDialogueTrigger[] dialogues = GetComponents<TutorialDialogueTrigger>();
        foreach(TutorialDialogueTrigger dialogue in dialogues)
        {
            _dialogues.Enqueue(dialogue);
        }

        _sentences = new Queue<string>();

        StartDialogue();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnWaveFinished += StartDialogue;
        // can't do on disable because I want it to be called when not active :)
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);

        Debug.Log("hi");
        TutorialDialogueTrigger dialogue = _dialogues.Dequeue();
        
        _sentences.Clear();

        string[] sentences = dialogue.GetDialogue();

        foreach (string sentence in sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        _text.text = sentence;
    }

    private void EndDialogue()
    {
        gameObject.SetActive(false);
    }
}
