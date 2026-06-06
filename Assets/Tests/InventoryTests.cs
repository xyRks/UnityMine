using NUnit.Framework;
using UnityEngine;

public class InventoryTests
{
    private Inventory inventory;

    [SetUp]
    public void SetUp()
    {
        // Создаем новый GameObject и добавляем к нему компонент Inventory, либо инстанцируем напрямую (зависит от Unity моков)
        inventory = new GameObject().AddComponent<Inventory>();

        // Inventory Awake() usually handles setup, but if we don't have true Unity lifecycle, we do it here.
        inventory.slots = new InventorySlot[20];
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            inventory.slots[i] = new InventorySlot();
        }
    }

    [Test]
    public void RemoveItem_NegativeSlotIndex_DoesNotThrowException()
    {
        // Arrange
        int invalidSlotIndex = -1;
        int amountToRemove = 1;

        // Act & Assert
        Assert.DoesNotThrow(() => inventory.RemoveItem(invalidSlotIndex, amountToRemove));
    }

    [Test]
    public void RemoveItem_OutOfBoundsSlotIndex_DoesNotThrowException()
    {
        // Arrange
        int invalidSlotIndex = 25; // Greater than slots.Length (20)
        int amountToRemove = 1;

        // Act & Assert
        Assert.DoesNotThrow(() => inventory.RemoveItem(invalidSlotIndex, amountToRemove));
    }

    [Test]
    public void RemoveItem_ValidSlotIndex_RemovesAmount()
    {
        // Arrange
        ItemData mockItem = ScriptableObject.CreateInstance<ItemData>();
        mockItem.itemName = "TestItem";
        mockItem.maxStackSize = 10;

        inventory.slots[0].item = mockItem;
        inventory.slots[0].amount = 5;

        // Act
        inventory.RemoveItem(0, 2);

        // Assert
        Assert.AreEqual(3, inventory.slots[0].amount);
        Assert.AreEqual(mockItem, inventory.slots[0].item);
    }
}
