using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class VolumeSliderSetting : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField][Range(0, 100)] private float _initialValue = 100f;
    private float _minimumValue = 0.0001f;
    private float _maximumValue = 1f;
    [SerializeField] private string _settingText = "Default";
    [SerializeField] private string _audioMixerName = "Master";
    private AudioMixerGroupController _audioMixerGroupController;

    private void Awake()
    {
        _audioMixerGroupController = FindObjectOfType<AudioMixerGroupController>();
        _nameText.text = _settingText;
        _slider.minValue = _minimumValue;
        _slider.maxValue = _maximumValue;
    }

    private void Start()
    {
        _slider.value = (float)_initialValue / 100f;
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
        _valueText.text = ((int) (newValue * 100)).ToString();
        _audioMixerGroupController.SetAudioMixerVolume(_audioMixerName, newValue);
    }
}
