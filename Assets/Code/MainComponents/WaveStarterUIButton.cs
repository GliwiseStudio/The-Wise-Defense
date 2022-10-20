using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WaveStarterUIButton : MonoBehaviour
{
    private Button _startWaveButton;

    private void Awake()
    {
        _startWaveButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _startWaveButton.onClick.AddListener(StartWave);
        GameManager.Instance.OnWaveStarted += HideButton;
        GameManager.Instance.OnWaveFinished += ShowButton;
    }

    private void OnDisable()
    {
        _startWaveButton.onClick.RemoveListener(StartWave);
        GameManager.Instance.OnWaveStarted -= HideButton;
        GameManager.Instance.OnWaveFinished -= ShowButton;
    }

    private void StartWave()
    {
        GameManager.Instance.StartWave();
    }

    private void HideButton()
    {
        _startWaveButton.gameObject.SetActive(false);
    }

    private void ShowButton()
    {
        _startWaveButton.gameObject.SetActive(true);
    }
}
