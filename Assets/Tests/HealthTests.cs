using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private GameObject? testObject;
    private Health? healthComponent;

    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject();
        healthComponent = testObject.AddComponent<Health>();

        // Start initializes maxHealth and currentHealth
        // Simulate Start method behavior for initialization:
        healthComponent.maxHealth = 100;
        healthComponent.currentHealth = 100;
    }

    [TearDown]
    public void TearDown()
    {
        if (testObject != null)
        {
            UnityEngine.Object.DestroyImmediate(testObject);
        }
    }

    [Test]
    public void TakeDamage_NegativeDamage_DoesNotChangeHealth()
    {
        // Arrange
        int initialHealth = 100;
        healthComponent!.maxHealth = initialHealth;
        healthComponent.currentHealth = initialHealth;

        // Act
        healthComponent.TakeDamage(-10);

        // Assert
        Assert.AreEqual(initialHealth, healthComponent.currentHealth, "Negative damage should not reduce health.");
    }

    [Test]
    public void TakeDamage_PositiveDamage_ReducesHealth()
    {
        // Arrange
        int initialHealth = 100;
        healthComponent!.maxHealth = initialHealth;
        healthComponent.currentHealth = initialHealth;

        // Act
        healthComponent.TakeDamage(20);

        // Assert
        Assert.AreEqual(80, healthComponent.currentHealth, "Positive damage should reduce health.");
    }

    [Test]
    public void TakeDamage_MoreDamageThanCurrentHealth_SetsHealthToZero()
    {
        // Arrange
        int initialHealth = 100;
        healthComponent!.maxHealth = initialHealth;
        healthComponent.currentHealth = initialHealth;

        // Act
        healthComponent.TakeDamage(150);

        // Assert
        Assert.AreEqual(0, healthComponent.currentHealth, "Damage exceeding current health should set health to 0.");
    }
}
