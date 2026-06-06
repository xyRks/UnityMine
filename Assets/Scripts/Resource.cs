using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResourceDamageEvent : UnityEvent<int> { }

public class Resource : MonoBehaviour
{
    public ResourceDamageEvent OnDamageTaken = new ResourceDamageEvent();

    public void TakeDamage(int damage)
    {
        OnDamageTaken?.Invoke(damage);
    }
}
