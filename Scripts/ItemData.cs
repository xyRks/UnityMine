using UnityEngine;

// Типы предметов. Мы храним только инструменты, оружие и ресурсы.
// Броня и одежда запрещены правилами!
public enum ItemType
{
    Tool,    // Инструмент (например, кирка)
    Weapon,  // Оружие (например, меч)
    Resource // Ресурс (например, дерево или камень)
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    // Название предмета (например, "Деревянный меч")
    public string itemName;

    // Тип предмета (выбираем из списка выше)
    public ItemType itemType;

    // Максимальное количество предметов в одной ячейке (стаке)
    public int maxStackSize = 64;
}
