using UnityEngine;

public class TutorialDialogueTrigger : MonoBehaviour
{
    [TextArea(1, 10)]
    [SerializeField] private string[] _sentences;

    public string[] GetDialogue()
    {
        return _sentences;
    }
}
