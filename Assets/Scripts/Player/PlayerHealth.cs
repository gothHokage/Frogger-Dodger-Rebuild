using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private DeathScreen _deathScreen;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDamaged += OnDamaged;
        _healthSystem.OnDeath += OnDeath;
    }

    private void OnDamaged(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Contact:
                G.CameraShake?.Shake(1f);
                break;
            case DamageType.Falling:
                G.CameraShake?.Shake(2f);
                break;
        }
    }

    private void OnDeath()
    {
        _deathScreen?.Show();
    }
}