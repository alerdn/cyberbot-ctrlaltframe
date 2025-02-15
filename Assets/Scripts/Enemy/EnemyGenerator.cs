using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [field: SerializeField] public List<Enemy> Enemies { get; private set; }

    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private AnimationCurve _spawnAmountCurve;
    [SerializeField] private float _waveDuration = 15f;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private List<Transform> _spawnPoints;

    private int _waveIndex;
    private float _nextWaveTime;
    private bool _generating;

    private void Start()
    {
        Enemies = new();
        _generating = true;
        _nextWaveTime = _waveDuration;
    }

    private void Update()
    {
        if (!_generating) return;

        _nextWaveTime -= Time.deltaTime;
        if (_nextWaveTime <= 0)
        {
            _nextWaveTime = _waveDuration;
            SpawnWave();
        }
    }

    public void SpawnWave()
    {
        _waveIndex++;
        int spawnAmount = Mathf.RoundToInt(_spawnAmountCurve.Evaluate(_waveIndex));

        spawnAmount = Mathf.Min(_maxEnemies - Enemies.Count, spawnAmount);

        for (int i = 0; i < spawnAmount; i++)
        {
            Transform spawnPoint = _spawnPoints.GetRandom();
            Enemy enemy = Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity, transform);
            enemy.OnDeathEvent += (enemy) => Enemies.Remove(enemy);
            Enemies.Add(enemy);
        }
    }

    public void Stop()
    {
        _generating = false;
    }
}
