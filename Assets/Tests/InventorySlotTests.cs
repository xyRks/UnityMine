using NUnit.Framework;
using UnityEngine;

// Тесты для ячейки инвентаря
[TestFixture]
public class InventorySlotTests
{
    // Проверяем, что метод Clear очищает предмет и ставит количество на 0
    [Test]
    public void Clear_ОчищаетПредметИКоличество()
    {
        // Подготовка (Arrange): создаем ячейку с предметом
        InventorySlot slot = new InventorySlot();
        slot.item = ScriptableObject.CreateInstance<ItemData>(); // Кладем пустой предмет для теста
        slot.amount = 10; // Ставим количество 10

        // Действие (Act): вызываем метод очистки
        slot.Clear();

        // Проверка (Assert): проверяем результаты
        Assert.That(slot.item, Is.Null, "Предмет должен исчезнуть (стать null) после очистки");
        Assert.That(slot.amount, Is.EqualTo(0), "Количество предметов должно стать 0 после очистки");
        Assert.That(slot.IsEmpty(), Is.True, "Ячейка должна считаться пустой после очистки");
    }
}
