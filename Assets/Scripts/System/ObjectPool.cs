using System.Collections.Generic;
using UnityEngine;

public class  ObjectPool : MonoBehaviour, IService
{
    private Dictionary<string, Queue<GameObject>> pools = new();
    private Dictionary<string, GameObject> prefabs = new();
    
    public void Init()
    {
        Debug.Log("ObjectPool init called");
    }
    
    public void RegisterPool(string key, GameObject prefab, int initialSize = 5)
    {
        if (pools.ContainsKey(key)) return;

        prefabs[key] = prefab;
        pools[key] = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
            pools[key].Enqueue(CreateNew(key, prefab));
    }
    
    public GameObject Get(string key, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogWarning($"Pool '{key}' not registered");
            return null;
        }

        GameObject obj;

        if (pools[key].Count > 0)
        {
            obj = pools[key].Dequeue();
        }
        else
        {
            obj = CreateNew(key, prefabs[key]);
        }
            
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Return(string key, GameObject obj)
    {
        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }

    private GameObject CreateNew(string key, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        
        var poolable = obj.GetComponent<IPoolable>();
        poolable?.SetPoolKey(key);
        return obj;
    }
    
    
}
