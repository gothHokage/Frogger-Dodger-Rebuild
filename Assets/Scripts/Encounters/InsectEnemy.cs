using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageDealer))]
public class InsectEnemy : PoolableObject
{
    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float wanderStrength = 1.5f;
    [SerializeField] private float wanderInterval = 0.8f;
    [SerializeField] private float separationRadius = 1.2f;
    [SerializeField] private float separationStrength = 2f;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 wanderOffset;
    private float wanderTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public override void OnSpawn()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        wanderOffset = Random.insideUnitCircle * wanderStrength;
        wanderTimer = 0;
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
    }

    public override void OnDespawn()
    {
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void FixedUpdate()
    {
        if (player == null || !rb.simulated) return;

        wanderTimer -= Time.fixedDeltaTime;
        if (wanderTimer <= 0)
        {
            wanderOffset = Random.insideUnitCircle * wanderStrength;
            wanderTimer = wanderInterval;
        }

        Vector2 desired = Seek() + Separation();
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desired, Time.fixedDeltaTime * 5f);

        if (rb.linearVelocity.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(rb.linearVelocity.x), 1f, 1f);
    }

    private Vector2 Seek()
    {
        Vector2 target = (Vector2)player.position + wanderOffset;
        return ((target - (Vector2)transform.position).normalized) * speed;
    }

    private Vector2 Separation()
    {
        Vector2 force = Vector2.zero;
        var colliders = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (var col in colliders)
        {
            if (col.gameObject == gameObject) continue;
            if (!col.TryGetComponent<InsectEnemy>(out _)) continue;

            Vector2 away = (Vector2)(transform.position - col.transform.position);
            float dist = away.magnitude;
            if (dist > 0)
                force += away.normalized / dist * separationStrength;
        }

        return force;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out _))
            ReturnToPool();
    }
}