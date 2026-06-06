using UnityEngine;

/// <summary>
/// Ячейка инвентаря.
/// Хранит данные о предмете и его количестве.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    [Tooltip("Предмет в ячейке")]
    public ItemData item;

    [Tooltip("Количество предметов в ячейке")]
    public int amount;

    /// <summary>
    /// Очищает ячейку (удаляет предмет и сбрасывает количество).
    /// </summary>
    public void Clear()
    {
        item = null;
        amount = 0;
    }

    /// <summary>
    /// Проверяет, пуста ли ячейка.
    /// </summary>
    /// <returns>Возвращает true, если ячейка пуста.</returns>
    public bool IsEmpty()
    {
        return item == null || amount <= 0;
    }
}
