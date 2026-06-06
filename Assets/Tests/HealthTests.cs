using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private GameObject testObject;
    private Health healthComponent;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("TestObject");
        healthComponent = testObject.AddComponent<Health>();
        // Инициализируем начальные значения для тестов
        healthComponent.maxHealth = 100;
        healthComponent.currentHealth = 50;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(testObject);
    }

    [Test]
    public void Heal_DoesNotExceedMaxHealth()
    {
        // Act: лечим на значение, которое превысит maxHealth (50 + 60 = 110)
        healthComponent.Heal(60);

        // Assert: текущее здоровье не должно превышать максимальное
        Assert.That(healthComponent.currentHealth, Is.EqualTo(100), "Здоровье не должно превышать максимальное значение при лечении.");
    }

    [Test]
    public void Heal_IncreasesHealthCorrectly()
    {
        // Act: лечим на значение в пределах максимума (50 + 20 = 70)
        healthComponent.Heal(20);

        // Assert
        Assert.That(healthComponent.currentHealth, Is.EqualTo(70), "Здоровье должно корректно увеличиваться при лечении.");
    }

    [Test]
    public void Heal_NegativeOrZeroAmount_DoesNothing()
    {
        // Act: пытаемся вылечить отрицательным значением
        healthComponent.Heal(-10);

        // Assert
        Assert.That(healthComponent.currentHealth, Is.EqualTo(50), "Отрицательное лечение не должно изменять здоровье.");

        // Act: пытаемся вылечить нулевым значением
        healthComponent.Heal(0);

        // Assert
        Assert.That(healthComponent.currentHealth, Is.EqualTo(50), "Нулевое лечение не должно изменять здоровье.");
    }
}
