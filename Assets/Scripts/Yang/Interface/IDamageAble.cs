using System;

public interface IDamageAble<T> {

    void Damage(T damageTaken);
    void Dead();
    void Move();
    void Attack(object sender, EventArgs e);
    void Shot();
}
