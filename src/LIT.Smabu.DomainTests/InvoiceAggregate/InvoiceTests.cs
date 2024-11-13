using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;

namespace LIT.Smabu.DomainTests.InvoiceAggregate
{
    [TestClass]
    public class InvoiceTests
    {
        private readonly InvoiceId _invoiceId = new(Guid.NewGuid());
        private readonly CustomerId _customerId = new(Guid.NewGuid());
        private readonly Address _address = new("Name 1", "Name 2", "Street", "House number", "12345", "City", "Country");
        private readonly DatePeriod _datePeriod = DatePeriod.Create(DateTime.Now, DateTime.Now.AddDays(1));
        private readonly Currency _currency = Currency.EUR;
        private readonly TaxRate _taxRate = new("Default", 19, "Tax");

        [TestMethod]
        public void Create_ShouldReturnInvoice()
        {
            // Act
            var testee = Invoice.Create(_invoiceId, _customerId, 2023, _address, _datePeriod, _currency, _taxRate);

            // Assert
            Assert.IsNotNull(testee);
            Assert.AreEqual(_invoiceId, testee.Id);
            Assert.AreEqual(_customerId, testee.CustomerId);
            Assert.AreEqual(2023, testee.FiscalYear);
            Assert.AreEqual(_address, testee.CustomerAddress);
            Assert.AreEqual(_datePeriod, testee.PerformancePeriod);
            Assert.AreEqual(_currency, testee.Currency);
            Assert.AreEqual(_taxRate, testee.TaxRate);
            Assert.IsFalse(testee.IsReleased);
            Assert.IsNull(testee.ReleasedAt);
            Assert.IsNull(testee.InvoiceDate);
            Assert.AreEqual(0, testee.Items.Count);
        }

        [TestMethod()]
        public void CreateFromTemplate_ShouldReturnSameAmountAsTemplate()
        {
            // Arrange
            var template = Invoice.Create(new(Guid.NewGuid()), new(Guid.NewGuid()), 2023, _address, _datePeriod, _currency, _taxRate);
            template.AddItem(new(Guid.NewGuid()), "Detail1", new(2, Unit.Item), 99.50m, null);
            template.AddItem(new(Guid.NewGuid()), "Detail2", new(8, Unit.Hour), 120m, null);
            template.AddItem(new(Guid.NewGuid()), "Detail3", new(2, Unit.Hour), 120m, null);

            // Act
            var testee = Invoice.CreateFromTemplate(_invoiceId, _customerId, 2024, _address, _datePeriod, template);

            // Assert
            Assert.IsNotNull(testee);
            Assert.AreEqual(_customerId, testee.CustomerId);
            Assert.AreEqual(2024, testee.FiscalYear);
            Assert.AreEqual(_datePeriod, testee.PerformancePeriod);
            Assert.AreEqual(_currency, testee.Currency);
            Assert.IsFalse(testee.IsReleased);
            Assert.AreEqual(template.Items.Count, testee.Items.Count);
            Assert.AreEqual(template.Amount, testee.Amount);
            Assert.AreNotEqual(template.Id, testee.Id);
            Assert.AreNotEqual(template.Items[0].Id, testee.Items[0].Id);
        }

        [TestMethod]
        public void AddItem_ShouldAddItemToInvoice()
        {
            // Arrange
            var testee = Invoice.Create(_invoiceId, _customerId, 2023, _address, _datePeriod, _currency, _taxRate);
            var itemId = new InvoiceItemId(Guid.NewGuid());
            var details = "Item Details";
            var quantity = new Quantity(1, Unit.Item);
            var unitPrice = 100m;

            // Act
            var result = testee.AddItem(itemId, details, quantity, unitPrice);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, testee.Items.Count);
            Assert.AreEqual(details, testee.Items.First().Details);
        }

        [TestMethod]
        public void Update_ShouldUpdateInvoice()
        {
            // Arrange
            var testee = Invoice.Create(_invoiceId, _customerId, 2023, _address, _datePeriod, _currency, _taxRate);
            var newDatePeriod = DatePeriod.Create(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));
            var newTax = new TaxRate("New", 1, "New tax");
            var newInvoiceDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = testee.Update(newDatePeriod, newTax, newInvoiceDate);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(newDatePeriod, testee.PerformancePeriod);
            Assert.AreEqual(newTax, testee.TaxRate);
            Assert.AreEqual(newInvoiceDate, testee.InvoiceDate);
        }

        [TestMethod]
        public void Release_ShouldReleaseInvoice()
        {
            // Arrange
            var testee = Invoice.Create(_invoiceId, _customerId, 2023, _address, _datePeriod, _currency, _taxRate);
            var number = InvoiceNumber.CreateFirst(2023);
            var releasedAt = DateTime.Now;
            testee.AddItem(new(Guid.NewGuid()), "Details", new(1, Unit.Item), 1);

            // Act
            var result = testee.Release(number, releasedAt);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(testee.IsReleased);
            Assert.AreEqual(number, testee.Number);
            Assert.AreEqual(releasedAt, testee.ReleasedAt);
        }

        [TestMethod]
        public void WithdrawRelease_ShouldWithdrawRelease()
        {
            // Arrange
            var testee = Invoice.Create(_invoiceId, _customerId, 2023, _address, _datePeriod, _currency, _taxRate);
            var number = InvoiceNumber.CreateFirst(2023);
            testee.AddItem(new(Guid.NewGuid()), "Details", new(1, Unit.Item), 1);
            testee.Release(number, DateTime.Now);

            // Act
            var result = testee.WithdrawRelease();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(testee.IsReleased);
        }

        [TestMethod()]
        public void Delete_Invoice_Succeeds()
        {
            // Arrange
            var testee = Invoice.Create(_invoiceId, _customerId, 2023, _address, _datePeriod, _currency, _taxRate);

            // Act
            var result = testee.Delete();

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}