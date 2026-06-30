// RunStatsSystem.cs
using UnityEngine;
using System;

public class RunStatsSystem : MonoBehaviour, IService
{
    private const string _essenceSaveKey = "TotalEssence";

    public int CurrentEssence { get; private set; }
    public int CurrentKills { get; private set; }
    public float CurrentTime { get; private set; }

    public int TotalEssence { get; private set; }

    private bool _isRunning;

    public event Action<int> OnEssenceChanged;
    public event Action<int> OnKillsChanged;
    public event Action<float> OnTimeChanged;

    public void Init()
    {
        TotalEssence = PlayerPrefs.GetInt(_essenceSaveKey, 0);
        Debug.Log($"RunStatsSystem init, total essence: {TotalEssence}");
    }

    private void Update()
    {
        if (!_isRunning) return;
        CurrentTime += Time.deltaTime;
        OnTimeChanged?.Invoke(CurrentTime);
    }

    public void StartRun()
    {
        CurrentEssence = 0;
        CurrentKills = 0;
        CurrentTime = 0f;
        _isRunning = true;
    }

    public void EndRun()
    {
        _isRunning = false;
        TotalEssence += CurrentEssence;
        SaveTotalEssence();
    }

    public void AddEssence(int amount)
    {
        CurrentEssence += amount;
        OnEssenceChanged?.Invoke(CurrentEssence);
    }

    public void AddKill()
    {
        CurrentKills++;
        OnKillsChanged?.Invoke(CurrentKills);
    }

    public bool SpendEssence(int amount)
    {
        if (TotalEssence < amount) return false;
        TotalEssence -= amount;
        SaveTotalEssence();
        return true;
    }

    private void SaveTotalEssence()
    {
        PlayerPrefs.SetInt(_essenceSaveKey, TotalEssence);
        PlayerPrefs.Save();
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(CurrentTime / 60f);
        int seconds = Mathf.FloorToInt(CurrentTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}