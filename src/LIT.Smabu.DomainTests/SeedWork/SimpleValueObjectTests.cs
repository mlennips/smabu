using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;

namespace LIT.Smabu.DomainTests.SeedWork
{
    [TestClass()]
    public class SimpleValueObjectTests
    {
        [TestMethod()]
        public void Create_String_Succeeds()
        {
            // Arrange
            string helloText = "Hello";

            // Act
            var testee = new FakeStringValueObject(helloText);

            // Assert
            Assert.IsTrue(testee.Value == helloText);
            Assert.AreEqual(helloText.GetHashCode(), helloText.GetHashCode());
        }

        [TestMethod()]
        public void Compare_String_SameValues_ReturnsTrue()
        {
            // Arrange
            string helloText = "Hello";
            var testee = new FakeStringValueObject(helloText);
            var sameValueItem = new FakeStringValueObject(helloText);

            // Act

            // Assert
            Assert.IsTrue(testee == sameValueItem);
            Assert.IsFalse(testee != sameValueItem);
            Assert.IsTrue(testee.Equals(sameValueItem));
            Assert.AreEqual(testee, sameValueItem);
        }

        [TestMethod()]
        public void Compare_String_DifferentValues_ReturnsFalse()
        {
            // Arrange
            string helloText = "Hello";
            string otherText = "Moin";
            var testee = new FakeStringValueObject(helloText);
            var otherItem = new FakeStringValueObject(otherText);

            // Act

            // Assert
            Assert.IsFalse(testee == otherItem);
            Assert.IsTrue(testee != otherItem);
            Assert.IsFalse(testee.Equals(otherItem));
            Assert.AreNotEqual(testee, otherItem);
        }

        [TestMethod()]
        public void Create_Int_Succeeds()
        {
            // Arrange
            int number = 42;

            // Act
            var testee = new FakeIntValueObject(number);

            // Assert
            Assert.IsTrue(testee.Value == number);
            Assert.AreEqual(number.GetHashCode(), number.GetHashCode());
        }

        [TestMethod()]
        public void Compare_Int_SameValues_ReturnsTrue()
        {
            // Arrange
            int number = 42;
            var testee = new FakeIntValueObject(number);
            var sameValueItem = new FakeIntValueObject(number);

            // Act

            // Assert
            Assert.IsTrue(testee == sameValueItem);
            Assert.IsFalse(testee != sameValueItem);
            Assert.IsTrue(testee.Equals(sameValueItem));
            Assert.AreEqual(testee, sameValueItem);
        }

        [TestMethod()]
        public void Compare_Int_DifferentValues_ReturnsFalse()
        {
            // Arrange
            int number = 42;
            int otherNumber = 99;
            var testee = new FakeIntValueObject(number);
            var otherItem = new FakeIntValueObject(otherNumber);

            // Act

            // Assert
            Assert.IsFalse(testee == otherItem);
            Assert.IsTrue(testee != otherItem);
            Assert.IsFalse(testee.Equals(otherItem));
            Assert.AreNotEqual(testee, otherItem);
        }

        private record FakeStringValueObject(string Value) : SimpleValueObject<string>(Value) { }
        private record FakeIntValueObject(int Value) : SimpleValueObject<int>(Value) { }
    }
}