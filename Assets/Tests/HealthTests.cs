using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class HealthTests
    {
        private GameObject _healthGameObject;
        private Health _health;

        [SetUp]
        public void Setup()
        {
            _healthGameObject = new GameObject();
            _health = _healthGameObject.AddComponent<Health>();
            _health.maxHealth = 100;
            _health.currentHealth = 100;
        }

        [TearDown]
        public void Teardown()
        {
            UnityEngine.Object.DestroyImmediate(_healthGameObject);
        }

        [Test]
        public void TakeDamage_WithFatalDamage_SetsHealthToZeroAndFiresOnDeath()
        {
            // Arrange
            _health.currentHealth = 100;
            bool onDeathFired = false;
            _health.OnDeath.AddListener(() => onDeathFired = true);

            // Act
            _health.TakeDamage(150); // Fatal damage

            // Assert
            Assert.That(_health.currentHealth, Is.EqualTo(0), "Health should not go below 0");
            Assert.That(onDeathFired, Is.True, "OnDeath event should be fired when health reaches 0");
        }

        [Test]
        public void TakeDamage_WithNonFatalDamage_ReducesHealth()
        {
            // Arrange
            _health.currentHealth = 100;

            // Act
            _health.TakeDamage(25);

            // Assert
            Assert.That(_health.currentHealth, Is.EqualTo(75));
        }

        [Test]
        public void TakeDamage_NegativeDamage_DoesNothing()
        {
            // Arrange
            _health.currentHealth = 100;

            // Act
            _health.TakeDamage(-10);

            // Assert
            Assert.That(_health.currentHealth, Is.EqualTo(100));
        }
    }
}
