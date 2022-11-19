using System.Collections.Generic;
using UnityEngine;

public class CardStatsHolderUI : MonoBehaviour
{
    [SerializeField] private List<CardStatUI> _stats;
    private IconsStorage _iconsStorage;

    private void Awake()
    {
        _iconsStorage = FindObjectOfType<IconsStorage>();
    }

    public void SetStats(IReadOnlyList<CardStatConfiguration> stats)
    {
        foreach (CardStatUI statUI in _stats)
        {
            statUI.gameObject.SetActive(false);
        }

        Sprite sprite;
        for (int i = 0; i < stats.Count; i++)
        {
            sprite = _iconsStorage.GetSpriteFromCardStatType(stats[i].Name);
            _stats[i].SetSprite(sprite);
            _stats[i].SetValue(stats[i].Value.ToString());
            _stats[i].gameObject.SetActive(true);
        }
    }
}
