using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Событие изменения инвентаря.
/// Передает (Данные предмета, Количество, Индекс ячейки).
/// </summary>
[System.Serializable]
public class InventoryChangedEvent : UnityEvent<ItemData, int, int> { }

/// <summary>
/// Инвентарь игрока.
/// </summary>
public class Inventory : MonoBehaviour
{
    [Tooltip("Массив ячеек инвентаря")]
    public InventorySlot[] slots = new InventorySlot[20];

    [Tooltip("Событие, вызываемое при изменении инвентаря")]
    public InventoryChangedEvent OnInventoryChanged = new InventoryChangedEvent();

    private void Awake()
    {
        // Инициализация ячеек, если они еще не созданы
        if (slots == null || slots.Length == 0)
        {
            slots = new InventorySlot[20];
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new InventorySlot();
            }
        }
    }

    /// <summary>
    /// Удаляет предмет из указанной ячейки.
    /// </summary>
    /// <param name="slotIndex">Индекс ячейки.</param>
    /// <param name="amountToRemove">Количество для удаления.</param>
    public void RemoveItem(int slotIndex, int amountToRemove)
    {
        // Проверка граничных условий
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            return;
        }

        InventorySlot slot = slots[slotIndex];

        // Если ячейка пуста или запрашиваемое количество больше имеющегося
        if (slot.IsEmpty() || slot.amount < amountToRemove)
        {
            return;
        }

        slot.amount -= amountToRemove;

        // Если предметов не осталось, очищаем ячейку
        if (slot.amount <= 0)
        {
            slot.Clear();
        }

        // Вызываем событие изменения инвентаря
        if (OnInventoryChanged != null)
        {
            OnInventoryChanged.Invoke(slot.item, slot.amount, slotIndex);
        }
    }

    /// <summary>
    /// Добавляет предмет в инвентарь.
    /// </summary>
    /// <param name="itemToAdd">Предмет для добавления.</param>
    /// <param name="amountToAdd">Количество для добавления.</param>
    /// <returns>Возвращает true, если предмет успешно добавлен.</returns>
    public bool AddItem(ItemData itemToAdd, int amountToAdd)
    {
        if (itemToAdd == null || amountToAdd <= 0)
        {
            return false;
        }

        // 1. Ищем существующую ячейку с таким же предметом, где есть место
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].item == itemToAdd)
            {
                int spaceLeft = itemToAdd.maxStackSize - slots[i].amount;
                if (spaceLeft >= amountToAdd)
                {
                    slots[i].amount += amountToAdd;
                    if (OnInventoryChanged != null)
                    {
                        OnInventoryChanged.Invoke(slots[i].item, slots[i].amount, i);
                    }
                    return true;
                }
            }
        }

        // 2. Если не нашли существующую (или там нет места), ищем первую пустую ячейку
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].item = itemToAdd;
                slots[i].amount = amountToAdd;
                if (OnInventoryChanged != null)
                {
                    OnInventoryChanged.Invoke(slots[i].item, slots[i].amount, i);
                }
                return true;
            }
        }

        // Инвентарь полон
        Debug.Log("Инвентарь полон! Невозможно добавить " + itemToAdd.itemName);
        return false;
    }

    /// <summary>
    /// Подсчитывает общее количество указанного предмета в инвентаре.
    /// </summary>
    /// <param name="itemToCheck">Предмет для поиска.</param>
    /// <returns>Количество найденных предметов.</returns>
    public int CountItem(ItemData itemToCheck)
    {
        if (itemToCheck == null) return 0;

        int totalCount = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].item == itemToCheck)
            {
                totalCount += slots[i].amount;
            }
        }
        return totalCount;
    }

    /// <summary>
    /// Удаляет указанное количество предметов из инвентаря, проверяя все ячейки.
    /// </summary>
    /// <param name="itemToRemove">Предмет для удаления.</param>
    /// <param name="amountToRemove">Количество для удаления.</param>
    public void RemoveItems(ItemData itemToRemove, int amountToRemove)
    {
        if (itemToRemove == null || amountToRemove <= 0) return;

        int remainingToRemove = amountToRemove;

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].item == itemToRemove)
            {
                if (slots[i].amount >= remainingToRemove)
                {
                    // В этой ячейке достаточно предметов
                    RemoveItem(i, remainingToRemove);
                    return; // Успешно удалили все
                }
                else
                {
                    // В этой ячейке меньше, чем нужно, удаляем все что есть и продолжаем искать
                    remainingToRemove -= slots[i].amount;
                    RemoveItem(i, slots[i].amount);
                }
            }
        }
    }
}
