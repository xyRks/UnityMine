using UnityEngine;
using NUnit.Framework;

public class HealthTests
{
    private GameObject testGameObject;
    private Health healthComponent;

    [SetUp]
    public void Setup()
    {
        testGameObject = new GameObject();
        healthComponent = testGameObject.AddComponent<Health>();
        healthComponent.maxHealth = 100;
        healthComponent.currentHealth = 100;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(testGameObject);
    }

    [Test]
    public void TakeDamage_NormalDamage_ReducesHealth()
    {
        // Act
        healthComponent.TakeDamage(20);

        // Assert
        Assert.AreEqual(80, healthComponent.currentHealth);
    }

    [Test]
    public void TakeDamage_NegativeDamage_DoesNothing()
    {
        // Act
        healthComponent.TakeDamage(-10);

        // Assert
        Assert.AreEqual(100, healthComponent.currentHealth);
    }

    [Test]
    public void TakeDamage_ZeroDamage_DoesNothing()
    {
        // Act
        healthComponent.TakeDamage(0);

        // Assert
        Assert.AreEqual(100, healthComponent.currentHealth);
    }

    [Test]
    public void TakeDamage_DamageExceedingCurrentHealth_ClampsToZero()
    {
        // Act
        healthComponent.TakeDamage(150);

        // Assert
        Assert.AreEqual(0, healthComponent.currentHealth);
    }

    [Test]
    public void TakeDamage_WhenDead_DoesNothing()
    {
        // Arrange
        healthComponent.TakeDamage(100); // Kills the entity
        Assert.AreEqual(0, healthComponent.currentHealth);

        // Act
        healthComponent.TakeDamage(50);

        // Assert
        Assert.AreEqual(0, healthComponent.currentHealth);
    }
}
