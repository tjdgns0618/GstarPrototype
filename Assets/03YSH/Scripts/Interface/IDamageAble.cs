using System;

public interface IDamageAble<T> {

    void Damage(T damageTaken);
    void Dead();
    void Attack();
    void Shot();

    void PlayKnockback(UnityEngine.Vector3 direction, float knockbackPower, float knockbackDuration);
}
