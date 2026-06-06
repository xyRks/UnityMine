using UnityEngine;

/// <summary>
/// Верстак для крафта инструментов и оружия.
/// Взаимодействует с инвентарем игрока и проверяет наличие ресурсов.
/// </summary>
public class CraftingBench : MonoBehaviour
{
    [Tooltip("Ссылка на инвентарь игрока")]
    public Inventory playerInventory;

    [Header("Ресурсы (Настроить в Инспекторе)")]
    public ItemData woodItem;
    public ItemData stoneItem;

    [Header("Результаты крафта (Настроить в Инспекторе)")]
    public ItemData pickaxeItem;
    public ItemData axeItem;
    public ItemData swordItem;
    public ItemData bowItem;

    /// <summary>
    /// Этот метод вызывается кнопкой "Скрафтить кирку"
    /// </summary>
    public void CraftPickaxe()
    {
        if (playerInventory == null || woodItem == null || stoneItem == null || pickaxeItem == null)
        {
            Debug.LogError("CraftingBench: Не настроены ссылки на инвентарь или предметы!");
            return;
        }

        // Проверяем, есть ли у игрока нужные ресурсы (2 дерева, 3 камня)
        if (playerInventory.CountItem(woodItem) >= 2 && playerInventory.CountItem(stoneItem) >= 3)
        {
            // Пытаемся добавить предмет ПЕРЕД тем как забрать ресурсы
            // Это предотвращает потерю ресурсов, если инвентарь полон
            if (playerInventory.AddItem(pickaxeItem, 1))
            {
                // Если добавление успешно, забираем ресурсы
                playerInventory.RemoveItems(woodItem, 2);
                playerInventory.RemoveItems(stoneItem, 3);
                Debug.Log("Вы скрафтили кирку!");
            }
        }
        else
        {
            Debug.Log("Не хватает ресурсов для кирки!");
        }
    }

    /// <summary>
    /// Этот метод вызывается кнопкой "Скрафтить топор"
    /// </summary>
    public void CraftAxe()
    {
        if (playerInventory == null || woodItem == null || stoneItem == null || axeItem == null) return;

        if (playerInventory.CountItem(woodItem) >= 3 && playerInventory.CountItem(stoneItem) >= 2)
        {
            if (playerInventory.AddItem(axeItem, 1))
            {
                playerInventory.RemoveItems(woodItem, 3);
                playerInventory.RemoveItems(stoneItem, 2);
                Debug.Log("Вы скрафтили топор!");
            }
        }
        else
        {
            Debug.Log("Не хватает ресурсов для топора!");
        }
    }

    /// <summary>
    /// Этот метод вызывается кнопкой "Скрафтить меч"
    /// </summary>
    public void CraftSword()
    {
         if (playerInventory == null || woodItem == null || stoneItem == null || swordItem == null) return;

        if (playerInventory.CountItem(woodItem) >= 1 && playerInventory.CountItem(stoneItem) >= 4)
        {
             if (playerInventory.AddItem(swordItem, 1))
             {
                playerInventory.RemoveItems(woodItem, 1);
                playerInventory.RemoveItems(stoneItem, 4);
                Debug.Log("Вы скрафтили меч!");
             }
        }
        else
        {
            Debug.Log("Не хватает ресурсов для меча!");
        }
    }

    /// <summary>
    /// Этот метод вызывается кнопкой "Скрафтить лук"
    /// </summary>
    public void CraftBow()
    {
        if (playerInventory == null || woodItem == null || stoneItem == null || bowItem == null) return;

        if (playerInventory.CountItem(woodItem) >= 5 && playerInventory.CountItem(stoneItem) >= 1)
        {
             if (playerInventory.AddItem(bowItem, 1))
             {
                playerInventory.RemoveItems(woodItem, 5);
                playerInventory.RemoveItems(stoneItem, 1);
                Debug.Log("Вы скрафтили лук!");
             }
        }
        else
        {
            Debug.Log("Не хватает ресурсов для лука!");
        }
    }

    /// <summary>
    /// Метод для открытия UI крафта. Блокирует движение игрока и освобождает курсор.
    /// ЖЕСТКОЕ ПРАВИЛО: При активации любого UI окна, скрипты движения и обзора ДОЛЖНЫ блокироваться.
    /// </summary>
    public void OpenCraftingUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerInventory != null && playerInventory.gameObject != null)
        {
            PlayerMovement movement = playerInventory.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = false;
            }

            // Ищем MouseLook, он может быть на дочерней камере
            MouseLook look = playerInventory.gameObject.GetComponentInChildren<MouseLook>();
            if (look != null)
            {
                look.enabled = false;
            }
        }
    }

    /// <summary>
    /// Метод для закрытия UI крафта. Возвращает управление игроку.
    /// </summary>
    public void CloseCraftingUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInventory != null && playerInventory.gameObject != null)
        {
            PlayerMovement movement = playerInventory.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = true;
            }

            MouseLook look = playerInventory.gameObject.GetComponentInChildren<MouseLook>();
            if (look != null)
            {
                look.enabled = true;
            }
        }
    }
}
