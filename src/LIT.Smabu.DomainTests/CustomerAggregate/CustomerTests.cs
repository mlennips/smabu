using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.DomainTests.CustomerAggregate
{
    [TestClass()]
    public class CustomerTests
    {
        private readonly CustomerId fakeId = new(Guid.NewGuid());
        private readonly CustomerNumber fakeNumber = new(42);
        private readonly string fakeName = "fake name";
        private readonly string fakeIndustryBranch = "fake industry branch";

        private Customer testee = default!;

        [TestInitialize]
        public void TestInitialize()
        {
            testee = Customer.Create(fakeId, fakeNumber, fakeName, fakeIndustryBranch);
        }

        [TestMethod()]
        public void Create_Customer_Succeeds()
        {
            // Arrange

            // Act

            // Assert
            Assert.AreEqual(fakeNumber, testee.Number);
            Assert.AreEqual(fakeId, testee.Id);
            Assert.AreEqual(fakeName, testee.Name);
            Assert.AreEqual(fakeIndustryBranch, testee.IndustryBranch);
        }

        [TestMethod()]
        public void Update_Customer_Succeeds()
        {
            // Arrange
            string otherName = "99 name";
            string otherIndustryBranch = "99 industry branch";
            string otherVatId = "99";
            var otherPaymentMethod = PaymentMethod.DirectDebit;
            var otherPaymentCondition = PaymentCondition.Template30DaysNet14Days2PercentDiscount;

            // Act
            testee.Update(otherName,
                          otherIndustryBranch,
                          new Address("a", "b", "c", "d", "e", "f", "g"),
                          new("d@d.e", "012", "0123", "www.internet.de"),
                          CorporateDesign.CreateDefault(otherName),
                          otherVatId, otherPaymentMethod, otherPaymentCondition);

            // Assert
            Assert.AreEqual(otherName, testee.Name);
            Assert.AreEqual(otherIndustryBranch, testee.IndustryBranch);
            Assert.AreEqual(otherVatId, testee.VatId);
            Assert.AreEqual(otherPaymentMethod, testee.PreferredPaymentMethod);
            Assert.IsNotNull(testee.MainAddress);
            Assert.IsNotNull(testee.Communication);
            Assert.AreEqual(otherPaymentCondition, testee.PaymentCondition);
        }

        [TestMethod()]
        public void Delete_Customer_Succeeds()
        {
            // Arrange

            // Act
            var result = testee.Delete();

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}