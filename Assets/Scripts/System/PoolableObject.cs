using System;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour, IPoolable
{
    public string PoolKey { get; private set; }
    public event Action OnReturnedToPool;

    public void SetPoolKey(string key) => PoolKey = key;

    public void ReturnToPool()
    {
        OnDespawn();
        OnReturnedToPool?.Invoke();
        G.ObjectPool.Return(PoolKey, gameObject);
    }

    public virtual void OnSpawn() { }
    public virtual void OnDespawn() { }

    private void OnEnable() => OnSpawn();
}