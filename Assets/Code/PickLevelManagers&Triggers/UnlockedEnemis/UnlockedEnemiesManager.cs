using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockedEnemiesManager : MonoBehaviour
{
    private Queue<UnlockedEnemy> _enemies;
    [SerializeField] private Image _enemyImage;
    [SerializeField] private TextMeshProUGUI _enemyName;
    [SerializeField] private TextMeshProUGUI _enemyDescription;
    private LevelSelection _currentLevelSelection;

    void Awake()
    {
        _enemies = new Queue<UnlockedEnemy>();
        gameObject.SetActive(false);
    }

    public void ShowEnemies(UnlockedEnemy[] enemies, LevelSelection levelSelection)
    {
        _currentLevelSelection = levelSelection;
        _enemies.Clear();

        foreach (UnlockedEnemy enemy in enemies)
        {
            _enemies.Enqueue(enemy);
        }

        DisplayNextEnemy();
    }

    public void DisplayNextEnemy()
    {
        if (_enemies.Count == 0)
        {
            EndShow();
            return;
        }

        UnlockedEnemy enemy = _enemies.Dequeue();
        _enemyImage.sprite = enemy.Sprite;
        _enemyName.text = enemy.Name;
        _enemyDescription.text = enemy.Description;
    }

    private void EndShow()
    {
        _currentLevelSelection.GoToLevel(); // after showing the unlocked cards, go to level screen
    }
}
