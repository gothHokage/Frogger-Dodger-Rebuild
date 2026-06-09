using UnityEngine;

public static class G
{
    public static SceneLoader SceneLoader;
    public static AudioManager AudioManager;
    public static InputManager InputManager;
    public static ObjectPool ObjectPool;
    public static CameraShake CameraShake;
}

public static class GameBootstrap 
{
    private static bool _isInitialized = false;
    private static GameObject _serviceHolder;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    
    static void Init()
    {
        if (_isInitialized == true)
        {
            return;
        }

        _isInitialized = true;
        _serviceHolder = new GameObject("Services");
        Object.DontDestroyOnLoad(_serviceHolder);
        
        G.AudioManager = CreateSimpleService<AudioManager>();
        G.SceneLoader = CreateSimpleService<SceneLoader>();
        G.InputManager = CreateSimpleService<InputManager>();
        G.ObjectPool = CreateSimpleService<ObjectPool>();
        
        Debug.Log("Game Bootstrap initialized");
    }

    private static T CreateSimpleService<T>() where T : MonoBehaviour, IService
    {
        GameObject g = new GameObject(typeof(T).Name);
        g.transform.SetParent(_serviceHolder.transform);
        T service = g.AddComponent<T>();
        service.Init();
        return service;
    }
    
}

public interface IService
{
    void Init();
}
