using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private bool _destroyOnHit = false;
    [SerializeField] private float _knockbackForce = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var damageable)) return;

        damageable.TakeDamage(_damage, _damageType);

        if (_knockbackForce > 0 && other.TryGetComponent<Rigidbody2D>(out var rb))
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            rb.AddForce(dir * _knockbackForce, ForceMode2D.Impulse);
        }

        if (_destroyOnHit)
            Destroy(gameObject);
    }
}