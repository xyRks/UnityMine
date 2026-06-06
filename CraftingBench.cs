using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Класс для инвентаря игрока.
// Содержит количество ресурсов и список готовых предметов.
public class PlayerInventory : MonoBehaviour
{
    public int wood = 10;
    public int stone = 10;
    public List<string> items = new List<string>();
}

// Верстак для крафта инструментов и оружия
public class CraftingBench : MonoBehaviour
{
    // Ссылка на инвентарь игрока
    public PlayerInventory playerInventory;

    // Вспомогательный метод для крафта предмета (проверяем ресурсы, забираем их и выдаем предмет)
    private void TryCraft(int woodCost, int stoneCost, string itemName)
    {
        // Проверяем, есть ли у игрока нужные ресурсы
        if (playerInventory.wood >= woodCost && playerInventory.stone >= stoneCost)
        {
            // Забираем ресурсы
            playerInventory.wood -= woodCost;
            playerInventory.stone -= stoneCost;
            // Добавляем готовый предмет в инвентарь
            playerInventory.items.Add(itemName);

            // Выводим сообщение с правильным русским окончанием
            if (itemName == "Кирка") Debug.Log("Вы скрафтили кирку!");
            else if (itemName == "Топор") Debug.Log("Вы скрафтили топор!");
            else if (itemName == "Меч") Debug.Log("Вы скрафтили меч!");
            else if (itemName == "Лук") Debug.Log("Вы скрафтили лук!");
            else Debug.Log("Вы скрафтили " + itemName.ToLower() + "!");
        }
        else
        {
            // Выводим сообщение об ошибке с правильным русским окончанием
            if (itemName == "Кирка") Debug.Log("Не хватает ресурсов для кирки!");
            else if (itemName == "Топор") Debug.Log("Не хватает ресурсов для топора!");
            else if (itemName == "Меч") Debug.Log("Не хватает ресурсов для меча!");
            else if (itemName == "Лук") Debug.Log("Не хватает ресурсов для лука!");
            else Debug.Log("Не хватает ресурсов для " + itemName.ToLower() + "!");
        }
    }

    // Этот метод вызывается кнопкой "Скрафтить кирку"
    public void CraftPickaxe()
    {
        TryCraft(2, 3, "Кирка");
    }

    // Этот метод вызывается кнопкой "Скрафтить топор"
    public void CraftAxe()
    {
        TryCraft(3, 2, "Топор");
    }

    // Этот метод вызывается кнопкой "Скрафтить меч"
    public void CraftSword()
    {
        TryCraft(1, 4, "Меч");
    }

    // Этот метод вызывается кнопкой "Скрафтить лук"
    public void CraftBow()
    {
        TryCraft(5, 1, "Лук");
    }
}
