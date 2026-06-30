// RunStatsUI.cs
using UnityEngine;
using TMPro;

public class RunStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _essenceText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Awake()
    {
        G.RunStatsSystem.OnEssenceChanged += essence => _essenceText.text = $"{essence}";
        G.RunStatsSystem.OnKillsChanged += kills => _killsText.text = $"{kills}";
        G.RunStatsSystem.OnTimeChanged += time => _timeText.text = G.RunStatsSystem.GetFormattedTime();
    }

    private void Start()
    {
        _essenceText.text = "0";
        _killsText.text = "0";
        _timeText.text = "00:00";
    }

    private void OnDestroy()
    {
        G.RunStatsSystem.OnEssenceChanged -= essence => _essenceText.text = $"{essence}";
        G.RunStatsSystem.OnKillsChanged -= kills => _killsText.text = $"{kills}";
        G.RunStatsSystem.OnTimeChanged -= time => _timeText.text = G.RunStatsSystem.GetFormattedTime();
    }
}