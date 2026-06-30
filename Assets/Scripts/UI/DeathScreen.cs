using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI _essenceText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Start()
    {
        panel.SetActive(false);
        
    }

    public void Show()
    {
        G.RunStatsSystem.EndRun();
        Time.timeScale = 0f;
        panel.SetActive(true);

        _essenceText.text = $"{G.RunStatsSystem.CurrentEssence}";
        _killsText.text = $"{G.RunStatsSystem.CurrentKills}";
        _timeText.text = G.RunStatsSystem.GetFormattedTime();
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        G.RunStatsSystem.StartRun();
        G.SceneLoader.ReloadScene();
    }

    public void OnWitchMenuButton()
    {
        Time.timeScale = 1f;
        G.SceneLoader.LoadWitchMenu();
    }
}