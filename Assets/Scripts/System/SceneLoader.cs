using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IService
{
    public void Init()
    {
        Debug.Log("SceneLoader init called");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadWitchMenu()
    {
        SceneManager.LoadScene("TheWitchHouse");
    }
    
}