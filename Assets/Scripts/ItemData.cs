using UnityEngine;

/// <summary>
/// Данные о предмете.
/// Используем ScriptableObject, чтобы легко создавать предметы в редакторе (Правая кнопка мыши -> Create -> Inventory -> Item).
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Tooltip("Название предмета")]
    public string itemName = "Новый предмет";

    [Tooltip("Максимальное количество предметов в одной ячейке")]
    public int maxStackSize = 64;
}
