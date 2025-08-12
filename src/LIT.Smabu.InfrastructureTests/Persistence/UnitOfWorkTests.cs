using LIT.Smabu.Core;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Infrastructure.Caching;
using LIT.Smabu.Infrastructure.Messaging;
using LIT.Smabu.Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.InfrastructureTests.Persistence
{
    [TestClass()]
    public class UnitOfWorkTests
    {
        private Mock<IAggregateRepository> mockAggregateRepository = default!;
        private Mock<IDomainEventDispatcher> mockDomainEventDispatcher = default!;
        private UnitOfWork testee = default!;

        [TestInitialize]
        public void Initialize()
        {
            mockAggregateRepository = new Mock<IAggregateRepository>();
            mockDomainEventDispatcher = new Mock<IDomainEventDispatcher>();

            testee = new UnitOfWork(mockAggregateRepository.Object, mockDomainEventDispatcher.Object);
        }

        [TestMethod()]
        public async Task Commit_WhenCreatingMultipleAggregates_ShouldSucceed()
        {
            // Arrange
            int actualCount = 0;
            mockAggregateRepository.Setup(x => x.CreateAsync(It.IsAny<Catalog>()))
                .Callback(() => Interlocked.Increment(ref actualCount))
                .Returns(Task.CompletedTask);

            var aggregate1 = new Catalog(new CatalogId(Guid.NewGuid()), "A1", []);
            var aggregate2 = new Catalog(new CatalogId(Guid.NewGuid()), "A2", []);
            var aggregate3 = new Catalog(new CatalogId(Guid.NewGuid()), "A3", []);

            await testee.Repository.CreateAsync(aggregate1);
            await testee.Repository.CreateAsync(aggregate2);
            await testee.Repository.CreateAsync(aggregate3);

            // Act
            await testee.CommitAsync();

            // Assert
            Assert.AreEqual(3, actualCount);
        }

        [TestMethod()]
        public async Task Commit_WhenUpdatingMultipleAggregates_ShouldSucceed()
        {
            // Arrange
            int actualCount = 0;
            mockAggregateRepository.Setup(x => x.UpdateAsync(It.IsAny<Catalog>()))
                .Callback(() => Interlocked.Increment(ref actualCount))
                .Returns(Task.CompletedTask);

            var aggregate1 = new Catalog(new CatalogId(Guid.NewGuid()), "A1", []);
            var aggregate2 = new Catalog(new CatalogId(Guid.NewGuid()), "A2", []);
            var aggregate3 = new Catalog(new CatalogId(Guid.NewGuid()), "A3", []);

            await testee.Repository.UpdateAsync(aggregate1);
            await testee.Repository.UpdateAsync(aggregate2);
            await testee.Repository.UpdateAsync(aggregate3);

            // Act
            await testee.CommitAsync();

            // Assert
            Assert.AreEqual(3, actualCount);
        }

        [TestMethod()]
        public async Task Commit_WhenDeletingMultipleAggregates_ShouldSucceed()
        {
            // Arrange
            int actualCount = 0;
            mockAggregateRepository.Setup(x => x.DeleteAsync(It.IsAny<Catalog>()))
                .Callback(() => Interlocked.Increment(ref actualCount))
                .Returns(Task.CompletedTask);

            var aggregate1 = new Catalog(new CatalogId(Guid.NewGuid()), "A1", []);
            var aggregate2 = new Catalog(new CatalogId(Guid.NewGuid()), "A2", []);
            var aggregate3 = new Catalog(new CatalogId(Guid.NewGuid()), "A3", []);

            await testee.Repository.DeleteAsync(aggregate1);
            await testee.Repository.DeleteAsync(aggregate2);
            await testee.Repository.DeleteAsync(aggregate3);

            // Act
            await testee.CommitAsync();

            // Assert
            Assert.AreEqual(3, actualCount);
        }

        [TestMethod()]
        public async Task Commit_WhenReleasingInvoice_ShouldSucceed()
        {
            // Arrange
            var invoice = Invoice.Create(new InvoiceId(Guid.NewGuid()), new CustomerId(Guid.NewGuid()), 2025,
                new Address("name1", "name2", "street", "houseNumber", "postalCode", "city", "country"),
                new DatePeriod(DateOnly.FromDateTime(DateTime.Now), null),
                Currency.EUR, TaxRate.Default, PaymentCondition.Default);

            invoice.AddItem(new InvoiceItemId(Guid.NewGuid()), "Item1", new Quantity(1, Unit.Hour), 5);
            await testee.Repository.CreateAsync(invoice);
            invoice.Release(new InvoiceNumber(1234), DateTime.Now);
            await testee.Repository.UpdateAsync(invoice);
            int eventsCountBeforeCommit = invoice.GetUncommittedEvents(false).Count();

            // Act
            await testee.CommitAsync();

            // Assert
            Assert.AreEqual(0, invoice.GetUncommittedEvents(false).Count());
            Assert.AreEqual(1, eventsCountBeforeCommit);
        }
    }
}