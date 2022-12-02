using UnityEngine;
using UnityEngine.UI;

public class UnlockedEnemiesTrigger : MonoBehaviour
{
    [SerializeField] private UnlockedEnemiesManager _enemiesManager;

    [SerializeField] private UnlockedEnemy[] _enemies;

    public void TriggerUnlockedCards(LevelSelection levelSelection)
    {
        _enemiesManager.gameObject.SetActive(true);
        _enemiesManager.ShowEnemies(_enemies, levelSelection);
    }
}
