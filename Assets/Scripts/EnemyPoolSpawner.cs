using System.Collections.Generic;
using UnityEngine;

// Простой спавнер врагов с использованием Object Pooling (пула объектов).
// Это позволяет не создавать и не удалять объекты во время игры (что может тормозить ПК),
// а просто включать и выключать их.
public class EnemyPoolSpawner : MonoBehaviour
{
    // Префаб (шаблон) врага, которого мы будем создавать
    [Tooltip("Префаб врага для создания пула")]
    public GameObject enemyPrefab;

    // Количество врагов в пуле
    [Tooltip("Сколько врагов создать заранее")]
    public int poolSize = 10;

    // Список всех врагов в пуле
    private List<GameObject> pool;

    void Start()
    {
        // Создаем пустой список для хранения наших врагов
        pool = new List<GameObject>();

        // В начале игры создаем нужное количество врагов
        for (int i = 0; i < poolSize; i++)
        {
            // Создаем нового врага на сцене
            GameObject newEnemy = Instantiate(enemyPrefab);

            // Прячем врага (выключаем его)
            newEnemy.SetActive(false);

            // Добавляем созданного врага в наш список (пул)
            pool.Add(newEnemy);
        }
    }

    // Метод для появления врага в определенной точке
    public GameObject SpawnEnemy(Vector3 position)
    {
        // Ищем в списке первого выключенного (свободного) врага
        for (int i = 0; i < pool.Count; i++)
        {
            // Если враг не активен, значит мы можем его использовать
            if (!pool[i].activeInHierarchy)
            {
                // Перемещаем врага в нужную точку
                pool[i].transform.position = position;

                // Включаем врага, чтобы он появился в игре
                pool[i].SetActive(true);

                // Возвращаем найденного врага
                return pool[i];
            }
        }

        // Если все враги уже используются, ничего не возвращаем
        Debug.LogWarning("Нет свободных врагов в пуле!");
        return null;
    }

    // Метод для возврата врага обратно в пул (прячем его)
    public void ReturnEnemy(GameObject enemy)
    {
        // Просто выключаем врага, чтобы использовать его потом снова
        enemy.SetActive(false);
    }
}
