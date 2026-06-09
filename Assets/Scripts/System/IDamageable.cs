public interface IDamageable
{
    void TakeDamage(int amount, DamageType damageType);
    bool IsDead { get; }
}