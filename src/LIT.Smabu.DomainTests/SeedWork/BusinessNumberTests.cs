using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.DomainTests.SeedWork
{
    [TestClass()]
    public class BusinessNumberTests
    {
        internal readonly string shortFormValue = "FAKE";
        private readonly int digitsValue = 4;

        [TestMethod()]
        public void Create_Number_Succeeds()
        {
            // Arrange
            int businessNumberValue1 = 1;

            // Act
            var testee = new FakeBusinessNumber(businessNumberValue1);

            // Assert
            Assert.AreEqual(businessNumberValue1, testee.Value);
            Assert.AreEqual(shortFormValue, testee.ShortForm);
            Assert.AreEqual(digitsValue, testee.Digits);
            Assert.IsTrue(testee.DisplayName.StartsWith(shortFormValue));
            Assert.IsTrue(testee.DisplayName.EndsWith(businessNumberValue1.ToString(new string('0', digitsValue))));
            Assert.IsFalse(testee.IsTemporary);
        }

        [TestMethod()]
        public void Create_TemporaryNumber_Succeeds()
        {
            // Arrange
            int businessNumberValue1 = 0;

            // Act
            var testee = new FakeBusinessNumber(businessNumberValue1);

            // Assert
            Assert.IsTrue(testee.IsTemporary);
            Assert.IsTrue(testee.DisplayName.StartsWith(shortFormValue));
            Assert.IsTrue(testee.DisplayName.EndsWith("TEMP"));
        }

        [TestMethod()]
        public void AscOrdering_Numbers_Succeeds()
        {
            // Arrange
            var testees = new FakeBusinessNumber[]
            {
                new(1),
                new(3),
                new(2),
                new(5),
                new(4),
            };

            // Act
            var ascSorted = testees.OrderBy(x => x).ToList();

            // Assert
            Assert.AreSame(testees[0], ascSorted[0]);
            Assert.AreSame(testees[1], ascSorted[2]);
            Assert.AreSame(testees[2], ascSorted[1]);
            Assert.AreSame(testees[3], ascSorted[4]);
            Assert.AreSame(testees[4], ascSorted[3]);
        }

        [TestMethod()]
        public void DescOrdering_Numbers_Succeeds()
        {
            // Arrange
            var testees = new FakeBusinessNumber[]
            {
                new(1),
                new(3),
                new(2),
                new(5),
                new(4),
            };

            // Act
            var ascSorted = testees.OrderByDescending(x => x).ToList();

            // Assert
            Assert.AreSame(testees[0], ascSorted[4]);
            Assert.AreSame(testees[1], ascSorted[2]);
            Assert.AreSame(testees[2], ascSorted[3]);
            Assert.AreSame(testees[3], ascSorted[0]);
            Assert.AreSame(testees[4], ascSorted[1]);
        }

        private record FakeBusinessNumber : BusinessNumber
        {
            public FakeBusinessNumber(int value) : base(value)
            {

            }

            public override string ShortForm { get; } = "FAKE";

            public override int Digits { get; } = 4;
        }
    }
}