using UnityEngine;

/// <summary>
/// Тип ресурса
/// </summary>
public enum ResourceType
{
    Tree, // Дерево
    Rock, // Камень
    Ore   // Руда
}

/// <summary>
/// Скрипт ресурсного узла (дерево, камень, руда).
/// Хранит прочность и спавнит дроп при разрушении.
/// </summary>
public class ResourceNode : MonoBehaviour
{
    [Header("Настройки ресурса")]
    [Tooltip("Тип данного ресурса")]
    public ResourceType type;

    [Tooltip("Количество ударов до разрушения")]
    public int hitsToDestroy = 3;

    [Tooltip("Префаб, который появится после разрушения (дроп)")]
    public GameObject dropPrefab;

    /// <summary>
    /// Метод для получения урона по ресурсу
    /// </summary>
    public void TakeHit()
    {
        hitsToDestroy--;

        // Если прочность закончилась, разрушаем объект
        if (hitsToDestroy <= 0)
        {
            DestroyNode();
        }
    }

    /// <summary>
    /// Разрушение узла и спавн дропа
    /// </summary>
    private void DestroyNode()
    {
        // Проверяем, есть ли префаб для дропа
        if (dropPrefab != null)
        {
            // Создаем дроп на месте этого ресурса
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        // Удаляем сам объект ресурса со сцены
        Destroy(gameObject);
    }
}
