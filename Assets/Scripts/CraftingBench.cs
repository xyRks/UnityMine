using UnityEngine;
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

    // Этот метод вызывается кнопкой "Скрафтить кирку"
    public void CraftPickaxe()
    {
        // Проверяем, есть ли у игрока нужные ресурсы (2 дерева, 3 камня)
        if (playerInventory.wood >= 2 && playerInventory.stone >= 3)
        {
            // Забираем ресурсы
            playerInventory.wood -= 2;
            playerInventory.stone -= 3;
            // Добавляем готовую кирку
            playerInventory.items.Add("Кирка");
            Debug.Log("Вы скрафтили кирку!");
        }
        else
        {
            Debug.Log("Не хватает ресурсов для кирки!");
        }
    }

    // Этот метод вызывается кнопкой "Скрафтить топор"
    public void CraftAxe()
    {
        // Проверяем, есть ли ресурсы (3 дерева, 2 камня)
        if (playerInventory.wood >= 3 && playerInventory.stone >= 2)
        {
            playerInventory.wood -= 3;
            playerInventory.stone -= 2;
            playerInventory.items.Add("Топор");
            Debug.Log("Вы скрафтили топор!");
        }
        else
        {
            Debug.Log("Не хватает ресурсов для топора!");
        }
    }

    // Этот метод вызывается кнопкой "Скрафтить меч"
    public void CraftSword()
    {
        // Проверяем, есть ли ресурсы (1 дерево, 4 камня)
        if (playerInventory.wood >= 1 && playerInventory.stone >= 4)
        {
            playerInventory.wood -= 1;
            playerInventory.stone -= 4;
            playerInventory.items.Add("Меч");
            Debug.Log("Вы скрафтили меч!");
        }
        else
        {
            Debug.Log("Не хватает ресурсов для меча!");
        }
    }

    // Этот метод вызывается кнопкой "Скрафтить лук"
    public void CraftBow()
    {
        // Проверяем, есть ли ресурсы (5 дерева, 1 камень)
        if (playerInventory.wood >= 5 && playerInventory.stone >= 1)
        {
            playerInventory.wood -= 5;
            playerInventory.stone -= 1;
            playerInventory.items.Add("Лук");
            Debug.Log("Вы скрафтили лук!");
        }
        else
        {
            Debug.Log("Не хватает ресурсов для лука!");
        }
    }
}
