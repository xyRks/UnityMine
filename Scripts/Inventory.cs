using UnityEngine;

// Класс для хранения информации об одной ячейке инвентаря
[System.Serializable]
public class InventorySlot
{
    public ItemData item; // Какой предмет лежит в ячейке
    public int amount;    // Количество предметов (стак)

    // Проверяем, пустая ли ячейка
    public bool IsEmpty()
    {
        return item == null || amount <= 0;
    }

    // Очищаем ячейку
    public void Clear()
    {
        item = null;
        amount = 0;
    }
}

public class Inventory : MonoBehaviour
{
    // Массив всех ячеек в нашем инвентаре
    public InventorySlot[] slots = new InventorySlot[20];

    void Awake()
    {
        // Инициализируем пустые ячейки при запуске, если они не заданы в редакторе
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new InventorySlot();
            }
        }
    }

    // Метод добавления предмета в инвентарь
    // Возвращает количество предметов, которые не поместились (если инвентарь полон)
    public int AddItem(ItemData itemToAdd, int amountToAdd)
    {
        // Сначала пытаемся добавить к уже существующим предметам такого же типа (стакаем)
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].item == itemToAdd)
            {
                // Сколько еще можно положить в эту ячейку
                int spaceLeft = slots[i].item.maxStackSize - slots[i].amount;

                if (spaceLeft > 0)
                {
                    // Если места хватает для всех предметов
                    if (amountToAdd <= spaceLeft)
                    {
                        slots[i].amount += amountToAdd;
                        return 0; // Все предметы поместились
                    }
                    else
                    {
                        // Кладем сколько влезет, и идем искать следующую ячейку
                        slots[i].amount += spaceLeft;
                        amountToAdd -= spaceLeft;
                    }
                }
            }
        }

        // Если остались предметы, ищем пустые ячейки для них
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].item = itemToAdd;

                // Если предметов меньше максимального стака, кладем все
                if (amountToAdd <= itemToAdd.maxStackSize)
                {
                    slots[i].amount = amountToAdd;
                    return 0; // Все предметы поместились
                }
                else
                {
                    // Заполняем ячейку полностью и ищем следующую
                    slots[i].amount = itemToAdd.maxStackSize;
                    amountToAdd -= itemToAdd.maxStackSize;
                }
            }
        }

        // Возвращаем то, что не поместилось
        return amountToAdd;
    }

    // Метод удаления предметов из определенной ячейки
    public void RemoveItem(int slotIndex, int amountToRemove)
    {
        // Проверяем, существует ли такая ячейка
        if (slotIndex < 0 || slotIndex >= slots.Length) return;

        InventorySlot slot = slots[slotIndex];

        if (!slot.IsEmpty())
        {
            slot.amount -= amountToRemove;

            // Если предметов не осталось (или ушли в минус), очищаем ячейку
            if (slot.amount <= 0)
            {
                slot.Clear();
            }
        }
    }

    // Метод разделения стака (например, берем половину предметов из одной ячейки в другую)
    public void SplitStack(int sourceSlotIndex, int amountToSplit)
    {
        // Проверяем, существуют ли ячейки
        if (sourceSlotIndex < 0 || sourceSlotIndex >= slots.Length) return;

        InventorySlot sourceSlot = slots[sourceSlotIndex];

        // Если ячейка пуста или мы пытаемся взять больше, чем там есть, ничего не делаем
        if (sourceSlot.IsEmpty() || sourceSlot.amount <= amountToSplit || amountToSplit <= 0) return;

        // Ищем пустую ячейку для отделения
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                // Забираем предметы из исходной ячейки
                sourceSlot.amount -= amountToSplit;

                // Кладем их в новую пустую ячейку
                slots[i].item = sourceSlot.item;
                slots[i].amount = amountToSplit;

                return; // Успешно разделили
            }
        }

        // Если дошли сюда, значит нет пустых ячеек. Стак не разделится.
    }
}
