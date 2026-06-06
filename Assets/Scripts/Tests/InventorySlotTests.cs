using NUnit.Framework;

namespace Tests
{
    public class InventorySlotTests
    {
        [Test]
        public void IsEmpty_ReturnsTrue_WhenItemIsNullAndAmountIsZero()
        {
            // Arrange
            var slot = new InventorySlot();
            slot.item = null;
            slot.amount = 0;

            // Act
            bool result = slot.IsEmpty();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsEmpty_ReturnsTrue_WhenItemIsNotNullButAmountIsZeroOrLess()
        {
            // Arrange
            var slot = new InventorySlot();
            slot.item = new ItemData();

            // Test 0 amount
            slot.amount = 0;
            Assert.That(slot.IsEmpty(), Is.True);

            // Test negative amount
            slot.amount = -1;
            Assert.That(slot.IsEmpty(), Is.True);
        }

        [Test]
        public void IsEmpty_ReturnsTrue_WhenItemIsNullButAmountIsGreaterThanZero()
        {
            // Arrange
            var slot = new InventorySlot();
            slot.item = null;
            slot.amount = 5;

            // Act
            bool result = slot.IsEmpty();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsEmpty_ReturnsFalse_WhenItemIsNotNullAndAmountIsGreaterThanZero()
        {
            // Arrange
            var slot = new InventorySlot();
            slot.item = new ItemData();
            slot.amount = 1;

            // Act
            bool result = slot.IsEmpty();

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
