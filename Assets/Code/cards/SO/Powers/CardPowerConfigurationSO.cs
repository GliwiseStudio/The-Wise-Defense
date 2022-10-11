using UnityEngine;

public abstract class CardPowerConfigurationSO : ScriptableObject
{
    protected abstract ICardPower InitializePower();
    public ICardPower Power => InitializePower();
}
