public interface IPoolable
{
    string PoolKey { get; }
    void SetPoolKey(string key);
    void OnSpawn();
    void OnDespawn();
}