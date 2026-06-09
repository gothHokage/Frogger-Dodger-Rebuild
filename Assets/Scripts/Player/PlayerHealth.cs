// PlayerHealth.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HealthSystem))]
public class PlayerHealth : MonoBehaviour
{
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamaged += OnDamaged;
        healthSystem.OnDeath += OnDeath;
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
        Debug.Log("Лягушонок погиб");
    }
}