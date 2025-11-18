using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private EnemyView _simpleEnemyPrefab;
    [SerializeField] private Transform _pointSpawn;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothEffectTime = 0.25f;
    [SerializeField] private int _startEnemyHealth = 80;
    [SerializeField] private int _increase = 20;
    [SerializeField] private int _divider = 3;

    private EnemyView _enemyView;

    private void Awake()
    {
        _enemyView = Instantiate(_simpleEnemyPrefab, _pointSpawn.position, Quaternion.identity, _enemyContainer);
        _enemyView.Initialize(_text, _slider, _smoothEffectTime);
    }

    internal Vector3 GetSpawnPosition()
    {
        return _pointSpawn.position;
    }

    internal IEnemy Create(int level)
    {
        int health = CalculateHealth(level);
        SimpleEnemyModel enemy = new SimpleEnemyModel(health, _pointSpawn.position);
        _enemyView.SetEnemy(enemy);

        return enemy;
    }

    private int CalculateHealth(int level)
    {
        if (level <= 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        int coefficient = level / _divider;

        return _startEnemyHealth + _increase * level + coefficient * _increase;
    }
}