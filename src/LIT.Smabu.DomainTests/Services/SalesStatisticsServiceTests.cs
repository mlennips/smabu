using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Core;
using Moq;

namespace LIT.Smabu.DomainTests.Services
{
    [TestClass()]
    public class SalesStatisticsServiceTests
    {
        private Mock<IAggregateStore> mockAggregateStore = default!;
        private SalesStatisticsService testee = default!;

        private readonly CustomerId customerId1 = new(Guid.NewGuid());
        private readonly CustomerId customerId2 = new(Guid.NewGuid());
        private readonly CustomerId customerId3 = new(Guid.NewGuid());

        [TestInitialize]
        public void Initialize()
        {
            mockAggregateStore = new Mock<IAggregateStore>();
            mockAggregateStore.Setup(x => x.GetAllAsync<Invoice>()).ReturnsAsync(
            [
                CreateInvoiceStub(customerId1, 2023, 1000),
                CreateInvoiceStub(customerId2, 2023, 1100),
                CreateInvoiceStub(customerId1, 2024, 2000),
                CreateInvoiceStub(customerId3, 2024, 2200),
            ]);
            testee = new SalesStatisticsService(mockAggregateStore.Object);
        }

        [TestMethod()]
        [DataRow(2000, 0.0)]
        [DataRow(2023, 2100.0)]
        [DataRow(2024, 4200.0)]
        public async Task CalculateSalesForYearAsync_ShouldReturnCorrectValue(int year, double expectedAmount)
        {
            // Arrange

            // Act
            var result = await testee.CalculateSalesForYearAsync(year);

            // Assert
            Assert.AreEqual((decimal)expectedAmount, result);
        }

        [TestMethod()]
        public async Task  CalculateSalesVolumeForLastMonthsAsync_ShouldReturnCorrectValue()
        {
            // Arrange
            uint months = 9999;

            // Act
            var result = await testee.CalculateSalesForLastMonthsAsync(months);

            // Assert
            Assert.AreEqual(6300, result);
        }

        [TestMethod()]
        public async Task CalculateTotalSalesAsync_ShouldReturnCorrectValue()
        {
            // Arrange

            // Act
            var result = await testee.CalculateTotalSalesAsync();

            // Assert
            Assert.AreEqual(6300, result);
        }

        [TestMethod()]
        public async Task CalculateSalesForCustomerAsync_ShouldReturnCorrectValue()
        {
            // Arrange
            var customerId = customerId1;

            // Act
            var result = await testee.CalculateSalesForCustomerAsync(customerId);

            // Assert
            Assert.AreEqual(3000, result);
        }

        [TestMethod()]
        public async Task GetHighestInvoicesAsync_ShouldReturnCorrectValue()
        {
            // Arrange

            // Act
            var result = await testee.GetHighestInvoicesAsync(2);

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(customerId3, result[0].CustomerId);
            Assert.AreEqual(customerId1, result[1].CustomerId);
        }

        [TestMethod()]
        public async Task GetSalesByYearAsync_ShouldReturnCorrectValue()
        {
            // Arrange

            // Act
            var result = await testee.GetSalesByYearAsync();

            // Assert
            Assert.AreEqual(2, result.Items.Length);
            Assert.AreEqual(2, result.Items[0].Customers.Length);
            Assert.AreEqual(2, result.Items[1].Customers.Length);
            Assert.AreEqual(2100, result.Items[0].Amount);
            Assert.AreEqual(4200, result.Items[1].Amount);
        }

        [TestMethod()]
        public async Task GetSalesByCustomerAsync_ShouldReturnCorrectValue()
        {
            // Arrange

            // Act
            var result = await testee.GetSalesByCustomerAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(3000, result[customerId1]);
            Assert.AreEqual(1100, result[customerId2]);
            Assert.AreEqual(2200, result[customerId3]);
        }

        private static Invoice CreateInvoiceStub(CustomerId customerId, int fiscalYear, decimal amount)
        {
            var invoiceId = new InvoiceId(Guid.NewGuid());
            var customerAddress = new Address("Name1", "Name2", "Street", "Housenumber", "PostalCode", "City", "Country");
            var performancePeriod = new DatePeriod(new DateOnly(2022, 1, 1), new DateOnly(2022, 1, 31));
            var currency = Currency.EUR;
            var taxRate = TaxRate.Default;

            var invoice = Invoice.Create(invoiceId, customerId, fiscalYear, customerAddress, performancePeriod, currency, taxRate);
            invoice.AddItem(new InvoiceItemId(Guid.NewGuid()), "Details", new Quantity(1, Unit.Item), amount);
            invoice.Meta = new AggregateMeta(1, new DateTime(fiscalYear, 1, 1), "1", "A", null, null, null);

            return invoice;
        }
    }
}