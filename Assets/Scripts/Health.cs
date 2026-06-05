using UnityEngine;

// Простой компонент здоровья для игрока
public class Health : MonoBehaviour
{
    public int currentHealth = 100;

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод смерти
    private void Die()
    {
        // Логика уничтожения объекта (можно расширить позже)
        Destroy(gameObject);
    }
}
