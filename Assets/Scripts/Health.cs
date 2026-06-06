using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Класс для события изменения здоровья
/// </summary>
[System.Serializable]
public class HealthChangedEvent : UnityEvent<int, int> { }

/// <summary>
/// Универсальный компонент здоровья для Игрока и Врагов.
/// Только отслеживание HP, без брони и прокачки.
/// При изменении здоровья вызывает OnHealthChanged, при смерти - OnDeath.
/// </summary>
public class Health : MonoBehaviour
{
    [Tooltip("Максимальное здоровье")]
    public int maxHealth = 100;

    [Tooltip("Текущее здоровье")]
    public int currentHealth;

    [Tooltip("Событие при изменении здоровья: передает (текущее, максимальное)")]
    public HealthChangedEvent OnHealthChanged = new HealthChangedEvent();

    [Tooltip("Событие при смерти (текущее здоровье <= 0)")]
    public UnityEvent OnDeath = new UnityEvent();

    private bool isDead = false;

    private void Start()
    {
        // Инициализация здоровья при старте, если оно не было задано
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Метод для получения урона
    /// </summary>
    /// <param name="damage">Количество урона (должно быть больше 0)</param>
    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0) return;

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Метод для лечения
    /// </summary>
    /// <param name="amount">Количество лечения (должно быть больше 0)</param>
    public void Heal(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Внутренний метод смерти
    /// </summary>
    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();

        // Логика уничтожения объекта
        Destroy(gameObject);
    }
}
