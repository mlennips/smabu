using LIT.Smabu.Core;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Infrastructure.Caching;
using LIT.Smabu.Infrastructure.Messaging;
using LIT.Smabu.Infrastructure.Persistence;
using LIT.Smabu.UseCases.Customers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.InfrastructureTests.Caching
{
    [TestClass()]
    public class AggregateCacheTests
    {
        private Mock<IAggregateRepository> mockAggregateRepository = default!;
        private Mock<IAggregateRepositoryFactory> mockAggregateRepositoryFactory = default!;
        private Mock<ILogger<AggregateCache>> mockLogger = default!;
        private AggregateCache testee = default!;

        [TestInitialize]
        public void Initialize()
        {
            mockAggregateRepository = new Mock<IAggregateRepository>();
            mockAggregateRepositoryFactory = new Mock<IAggregateRepositoryFactory>();
            mockAggregateRepositoryFactory.Setup(x => x.Create())
                .Returns(mockAggregateRepository.Object);
            mockLogger = new Mock<ILogger<AggregateCache>>();

            testee = new AggregateCache(mockLogger.Object, mockAggregateRepositoryFactory.Object);
        }

        [TestMethod()]
        public async Task Handle_AggregateCreated_ShouldAddNewItemToCache()
        {
            // Arrange
            var customer = CreateCustomer();

            // Act
            await testee.Handle(new InformativeNotification.AggregateCreatedEvent(customer), new CancellationToken());

            // Assert
            var cachedCustomer = await testee.GetByAsync(customer.Id);
            Assert.IsNotNull(cachedCustomer);
            Assert.AreEqual(customer.Id, cachedCustomer.Id);

        }

        [TestMethod()]
        public async Task Handle_AggregateUpdated_ShouldUpdateItemInCache()
        {
            // Arrange
            var customer = CreateCustomer();
            customer.Update("updatedName", customer.IndustryBranch, customer.MainAddress, customer.Communication,
                customer.CorporateDesign, customer.VatId, customer.PreferredPaymentMethod, customer.PaymentCondition);
            mockAggregateRepository.Setup(x => x.GetAllAsync<Customer>())
                .ReturnsAsync([CreateCustomer()]);

            // Act
            await testee.Handle(new InformativeNotification.AggregateUpdatedEvent(customer), new CancellationToken());

            // Assert
            var cachedCustomer = await testee.GetByAsync(customer.Id);
            Assert.IsNotNull(cachedCustomer);
            Assert.AreEqual(customer.Id, cachedCustomer.Id);
            Assert.AreEqual("updatedName", cachedCustomer.Name);
        }

        [TestMethod()]
        public async Task Handle_AggregateDeleted_ShouldDeleteItemInCache()
        {
            // Arrange
            var customer1 = CreateCustomer();
            var customer2 = CreateCustomer();
            mockAggregateRepository.Setup(x => x.GetAllAsync<Customer>())
                .ReturnsAsync([customer1, customer2]);

            // Act
            await testee.Handle(new InformativeNotification.AggregateDeletedEvent(customer1), new CancellationToken());

            // Assert
            var customerCount = await testee.CountAsync<Customer>();
            Assert.AreEqual(1, customerCount);
        }

        private static Customer CreateCustomer()
        {
            return new Customer(new CustomerId(Guid.NewGuid()), new CustomerNumber(1), "Name", "Branch", Currency.EUR,
                new Address("name1", "name2", "street", "1", "12345", "city", "country"),
                new Communication("email@email.de", "01234", "0124", "www.q.de"),
                new CorporateDesign("brand", "shortName", "slogan",
                Color.Create("#000000"), Color.Create("#111111"), null),
                "01", new Domain.PaymentAggregate.PaymentMethod("Cash"),
                PaymentCondition.Default);
        }
    }
}