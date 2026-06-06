using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private GameObject _gameObject = null!;
    private Health _health = null!;

    [SetUp]
    public void Setup()
    {
        _gameObject = new GameObject();
        _health = _gameObject.AddComponent<Health>();
        _health.maxHealth = 100;
        _health.currentHealth = 50;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(_gameObject);
    }

    [Test]
    public void Heal_IncreasesHealthCorrectly()
    {
        // Тест правильного увеличения здоровья
        _health.Heal(20);
        Assert.That(_health.currentHealth, Is.EqualTo(70));
    }

    [Test]
    public void Heal_DoesNotExceedMaxHealth()
    {
        // Тест того, что лечение не превышает максимальное здоровье
        _health.Heal(100);
        Assert.That(_health.currentHealth, Is.EqualTo(100));
    }

    [Test]
    public void Heal_NegativeOrZeroAmount_HasNoEffect()
    {
        // Тест того, что отрицательное или нулевое значение не влияет на здоровье
        _health.Heal(0);
        Assert.That(_health.currentHealth, Is.EqualTo(50));

        _health.Heal(-10);
        Assert.That(_health.currentHealth, Is.EqualTo(50));
    }

    [Test]
    public void Heal_WhenDead_HasNoEffect()
    {
        // Тест того, что мертвого персонажа нельзя вылечить
        _health.TakeDamage(100); // Убиваем персонажа (currentHealth становится 0, isDead = true)
        Assert.That(_health.currentHealth, Is.EqualTo(0));

        _health.Heal(50);
        Assert.That(_health.currentHealth, Is.EqualTo(0));
    }
}
