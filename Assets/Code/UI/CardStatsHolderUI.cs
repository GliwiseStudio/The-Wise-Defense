using UnityEngine;

public class CardStatsHolderUI : MonoBehaviour
{
    [SerializeField] private CardStatUI _damageStat;
    [SerializeField] private CardStatUI _firerateStat;
    [SerializeField] private CardStatUI _rangeStat;

    public void SetStats(int damageValue, float firerateValue, float rangeValue)
    {
        _damageStat.SetValue(damageValue);
        _firerateStat.SetValue(firerateValue);
        _rangeStat.SetValue(rangeValue);
    }
}
