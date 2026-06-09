using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image[] heartImages;

    [Header("Sprites")]
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    [Header("Pulse")]
    [SerializeField] private float _scaleMin = 0.8f;
    [SerializeField] private float _scaleMax = 0.85f;
    [SerializeField] private float _pulseSpeed = 2f;

    private void Awake()
    {
        healthSystem.OnHealthChanged += UpdateUI;
    }

    private void Start()
    {
        UpdateUI(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }

    private void Update()
    {
        float scale = Mathf.Lerp(_scaleMin, _scaleMax, (Mathf.Sin(Time.time * _pulseSpeed) + 1f) * 0.5f);
        foreach (var heart in heartImages)
            heart.transform.localScale = Vector3.one * scale;
    }

    private void UpdateUI(int current, int max)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = current - i * 2;

            if (heartValue >= 2)
                heartImages[i].sprite = heartFull;
            else if (heartValue == 1)
                heartImages[i].sprite = heartHalf;
            else
                heartImages[i].sprite = heartEmpty;
        }
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthChanged -= UpdateUI;
    }
}