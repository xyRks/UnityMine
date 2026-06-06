using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyDamageEvent : UnityEvent<int> { }

public class Enemy : MonoBehaviour
{
    public EnemyDamageEvent OnDamageTaken = new EnemyDamageEvent();

    public void TakeDamage(int damage)
    {
        OnDamageTaken?.Invoke(damage);
    }
}
