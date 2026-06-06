using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Простой контроллер интерфейса игры.
/// Отображает здоровье, инвентарь и управляет панелью крафта.
/// </summary>
public class GameUIController : MonoBehaviour
{
    [Tooltip("Ссылка на здоровье игрока")]
    public Health playerHealth;

    [Tooltip("Ссылка на инвентарь игрока")]
    public Inventory playerInventory;

    [Tooltip("Ссылка на верстак")]
    public CraftingBench craftingBench;

    [Header("Элементы UI")]
    [Tooltip("Текст для отображения здоровья")]
    public Text healthText;

    [Tooltip("Текст для отображения инвентаря")]
    public Text inventoryText;

    [Tooltip("Панель крафта")]
    public GameObject craftingPanel;

    void Start()
    {
        // Подписываемся на события изменения здоровья
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.AddListener(UpdateHealthUI);
            UpdateHealthUI(playerHealth.currentHealth, playerHealth.maxHealth);
        }

        // Подписываемся на события изменения инвентаря
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged.AddListener(UpdateInventoryUI);
            UpdateInventoryUI(null, 0, 0); // Начальное обновление
        }

        // Скрываем панель крафта при старте
        if (craftingPanel != null)
        {
            craftingPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Открытие/закрытие крафта по кнопке E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (craftingPanel != null)
            {
                if (craftingPanel.activeSelf)
                {
                    CloseCrafting();
                }
                else
                {
                    OpenCrafting();
                }
            }
        }
    }

    /// <summary>
    /// Открыть окно крафта
    /// </summary>
    public void OpenCrafting()
    {
        if (craftingPanel != null)
        {
            craftingPanel.SetActive(true);
            if (craftingBench != null)
            {
                craftingBench.OpenCraftingUI();
            }
        }
    }

    /// <summary>
    /// Закрыть окно крафта
    /// </summary>
    public void CloseCrafting()
    {
        if (craftingPanel != null)
        {
            craftingPanel.SetActive(false);
            if (craftingBench != null)
            {
                craftingBench.CloseCraftingUI();
            }
        }
    }

    private void UpdateHealthUI(int current, int max)
    {
        if (healthText != null)
        {
            healthText.text = $"Здоровье: {current} / {max}";
        }
    }

    private void UpdateInventoryUI(ItemData item, int amount, int index)
    {
        if (inventoryText != null && playerInventory != null)
        {
            string text = "Инвентарь:\n";
            bool isEmpty = true;
            for (int i = 0; i < playerInventory.slots.Length; i++)
            {
                var slot = playerInventory.slots[i];
                if (slot != null && !slot.IsEmpty())
                {
                    text += $"• {slot.item.itemName} x{slot.amount}\n";
                    isEmpty = false;
                }
            }
            if (isEmpty)
            {
                text += "(Пусто)";
            }
            inventoryText.text = text;
        }
    }
}
