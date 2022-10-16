using UnityEngine;
using UnityEngine.UI;

public class StartButtonControl : MonoBehaviour
{
    private Button _startButton;
    private bool _waitingToContinue = false;

    private void Awake()
    {
        _startButton = GetComponentInChildren<Button>();
    }

    private void Update()
    {
        if (_waitingToContinue)
        {
            if (!GameManager.Instance.GetIsWaveActive())
            {
                _waitingToContinue = false;
                _startButton.gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(NextWave);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(NextWave);
    }

    private void NextWave()
    {
        GameManager.Instance.StartWave();
        _startButton.gameObject.SetActive(false);
        _waitingToContinue = true;
    }
}
