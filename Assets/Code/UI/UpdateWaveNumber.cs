using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateWaveNumber : MonoBehaviour
{
    private TextMeshProUGUI _waveNumberText;

    void Start()
    {
        _waveNumberText = GetComponent<TextMeshProUGUI>();
        UpdateWaveNumberText();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnWaveFinished += UpdateWaveNumberText;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnWaveFinished -= UpdateWaveNumberText;
    }

    private void UpdateWaveNumberText()
    { 
        _waveNumberText.text = (GameManager.Instance.GetCurrentWave()+1).ToString() + "/" + (GameManager.Instance.GetTotalWaves()).ToString();
    }
}
