using UnityEngine;
using System;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private string _poolKey;
    [SerializeField] private int _poolSize = 5;

    [Header("Spawn")]
    [SerializeField] private DamageType _spawnerType;
    [SerializeField] private float _spawnIntervalMin = 2f;
    [SerializeField] private float _spawnIntervalMax = 5f;
    [SerializeField] private int _maxAlive = 3;

    private float _timer;
    private float _currentInterval;
    private int _aliveCount;

    private void Start()
    {
        if (string.IsNullOrEmpty(_poolKey))
        {
            Debug.LogError($"ObjectSpawner на {gameObject.name} — Pool Key пустой!");
            return;
        }

        G.ObjectPool.RegisterPool(_poolKey, _prefab, _poolSize);
        _currentInterval = GetRandomInterval();
    }

    private void Update()
    {
        if (_aliveCount >= _maxAlive) return;

        _timer += Time.deltaTime;
        if (_timer >= _currentInterval)
        {
            _timer = 0f;
            _currentInterval = GetRandomInterval();
            Spawn();
        }
    }

    private void Spawn()
    {
        var obj = G.ObjectPool.Get(_poolKey, transform.position, Quaternion.identity);
        if (obj == null) return;

        _aliveCount++;

        if (obj.TryGetComponent<PoolableObject>(out var poolable))
        {
            Action callback = null;
            callback = () =>
            {
                _aliveCount--;
                poolable.OnReturnedToPool -= callback;
            };
            poolable.OnReturnedToPool += callback;
        }
    }
    private float GetRandomInterval() => UnityEngine.Random.Range(_spawnIntervalMin, _spawnIntervalMax);
}