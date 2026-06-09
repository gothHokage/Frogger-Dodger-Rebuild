using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageDealer))]
public class FallingObject : PoolableObject
{
    [SerializeField] private float _gravityScale = 4f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void OnSpawn()
    {
        _rb.gravityScale = _gravityScale;
        _rb.linearVelocity = Vector2.zero;
    }

    public override void OnDespawn()
    {
        _rb.gravityScale = 0f;
        _rb.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out _))
        {
            ReturnToPool();
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            ReturnToPool();
    }
}