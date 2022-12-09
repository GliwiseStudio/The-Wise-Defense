using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class VolumeSliderSetting : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField][Range(0, 1)] private float _initialValue = 1f;
    private float _minimumValue = 0.0001f;
    private float _maximumValue = 1f;
    [SerializeField] private string _settingText = "Default";
    [SerializeField] private string _audioMixerName = "Master";
    [SerializeField] private string _volumeParameterName = "MasterVolume";
    private AudioSettingsController _audioSettingsController;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _audioSettingsController = FindObjectOfType<AudioSettingsController>();
        _nameText.text = _settingText;
        _slider.minValue = _minimumValue;
        _slider.maxValue = _maximumValue;

        UpdateText(_audioSettingsController.GetParameterFromAudioMixer(_audioMixerName, _volumeParameterName));
        UpdateSlider(_audioSettingsController.GetParameterFromAudioMixer(_audioMixerName, _volumeParameterName));
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(ChangeValue);
    }

    private void OnDisable()
    {
        _nameText.text = _settingText;
        _slider.onValueChanged.RemoveListener(ChangeValue);
    }

    private void ChangeValue(float newValue)
    {
        UpdateText(newValue);
        UpdateSlider(newValue);
        _audioSettingsController.ChangeAudioMixerParameter(_audioMixerName, _volumeParameterName, newValue);
    }

    private void UpdateText(float value)
    {
        _valueText.text = ((int)(value * 100)).ToString();
    }

    private void UpdateSlider(float value)
    {
        _slider.value = value;
    }
}
