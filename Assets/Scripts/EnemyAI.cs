using UnityEngine;
using UnityEngine.AI;

// Простой ИИ врага, преследующий игрока и наносящий урон
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player; // Ссылка на трансформ игрока
    public float detectionRadius = 15f; // Радиус видимости врага
    public float attackDistance = 2f; // Дистанция для атаки
    public int damage = 10; // Урон от одной атаки
    public float attackCooldown = 1.5f; // Пауза между атаками

    private NavMeshAgent agent; // Компонент навигации
    private float nextAttackTime = 0f; // Таймер следующей атаки

    private void Start()
    {
        // Получаем компонент NavMeshAgent при старте
        agent = GetComponent<NavMeshAgent>();

        // Если игрок не назначен в инспекторе, ищем его по тегу "Player"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        // Если игрок отсутствует, ничего не делаем
        if (player == null) return;

        // Вычисляем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем, находится ли игрок в зоне видимости (радиусе обнаружения)
        if (distanceToPlayer <= detectionRadius)
        {
            // Назначаем цель для движения агента
            agent.SetDestination(player.position);

            // Если мы достаточно близко для атаки, пробуем атаковать
            if (distanceToPlayer <= attackDistance)
            {
                TryAttack();
            }
        }
    }

    // Метод попытки атаки игрока
    private void TryAttack()
    {
        // Проверяем, прошел ли кулдаун атаки
        if (Time.time >= nextAttackTime)
        {
            // Обновляем время следующей возможной атаки
            nextAttackTime = Time.time + attackCooldown;

            // Пытаемся найти компонент Health у игрока
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Наносим урон игроку
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
