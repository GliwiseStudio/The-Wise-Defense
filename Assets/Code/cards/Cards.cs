using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    private Button button;
    public CardConfigurationSO cardConfig;

    public void Activate()
    {
        cardConfig.cardPower.Power.Activate();
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Activate);
    }

    private void OnDissable()
    {
        button.onClick.RemoveListener(Activate);
    }
}
