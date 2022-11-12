using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> _sentences;
    [SerializeField] private TextMeshProUGUI _text;
    private LevelSelection _currentLevelSelection;

    void Awake()
    {
        _sentences = new Queue<string>();
        gameObject.SetActive(false);
    }

    public void StartDialogue(string[] sentences, LevelSelection levelSelection)
    {
        _currentLevelSelection = levelSelection;
        _sentences.Clear();

        foreach(string sentence in sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(_sentences.Count == 0)
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
        _currentLevelSelection.ShowCardsNext(); // after the dialogue, the game shows the unlocked cards
    }
}
