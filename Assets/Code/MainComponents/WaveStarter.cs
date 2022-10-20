using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class WaveStarter : MonoBehaviour
{
    public event Action OnWaveStart;
    private Button _startWaveButton;

    private void Awake()
    {
        _startWaveButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _startWaveButton.onClick.AddListener(StartWave);
    }

    private void OnDisable()
    {
        _startWaveButton.onClick.RemoveListener(StartWave);
    }

    private void StartWave()
    {
        GameManager.Instance.StartWave();
        OnWaveStart?.Invoke();
    }
}
