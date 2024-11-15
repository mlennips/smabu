using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using Microsoft.VisualBasic;

namespace LIT.Smabu.DomainTests.PaymentAggregate
{
    [TestClass]
    public class PaymentTests
    {
        [TestMethod]
        public void CreateIncoming_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = new PaymentId(Guid.NewGuid());
            var number = new PaymentNumber(1234);
            var details = "Test Details";
            var payer = "Test Payer";
            var payee = "Test Payee";
            var customerId = new CustomerId(Guid.NewGuid());
            var invoiceId = new InvoiceId(Guid.NewGuid());
            var documentNr = "12345";
            var documentDate = DateTime.Now;
            var accountingDate = DateTime.Now;
            var amountDue = 100m;
            var dueDate = DateTime.Now.AddDays(30);
            var paymentMethod = PaymentMethod.Default;
            var paymentCondition = PaymentCondition.Default;

            // Act
            var payment = Payment.CreateIncoming(id, number, details, payer, payee, customerId, invoiceId, 
                documentNr, documentDate, accountingDate, amountDue, dueDate, paymentMethod, paymentCondition);

            // Assert
            Assert.AreEqual(id, payment.Id);
            Assert.AreEqual(number, payment.Number);
            Assert.AreEqual(PaymentDirection.Incoming, payment.Direction);
            Assert.AreEqual(details, payment.Details);
            Assert.AreEqual(payer, payment.Payer);
            Assert.AreEqual(payee, payment.Payee);
            Assert.AreEqual(customerId, payment.CustomerId);
            Assert.AreEqual(invoiceId, payment.InvoiceId);
            Assert.AreEqual(documentNr, payment.ReferenceNr);
            Assert.AreEqual(documentDate, payment.ReferenceDate);
            Assert.AreEqual(accountingDate, payment.AccountingDate);
            Assert.AreEqual(amountDue, payment.AmountDue);
            Assert.AreEqual(dueDate, payment.DueDate);
            Assert.AreEqual(0, payment.AmountPaid);
            Assert.AreEqual(Currency.EUR, payment.Currency);
            Assert.AreEqual(PaymentStatus.Pending, payment.Status);
            Assert.AreEqual(paymentMethod, payment.PaymentMethod);
            Assert.AreEqual(paymentCondition, payment.PaymentCondition);
        }

        [TestMethod]
        public void CreateOutgoing_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = new PaymentId(Guid.NewGuid());
            var number = new PaymentNumber(1234);
            var details = "Test Details";
            var payer = "Test Payer";
            var payee = "Test Payee";
            var documentNr = "12345";
            var documentDate = DateTime.Now;
            var accountingDate = DateTime.Now;
            var amountDue = 100m;
            var dueDate = DateTime.Now.AddDays(30);
            var paymentMethod = PaymentMethod.Default;
            var paymentCondition = PaymentCondition.Default;

            // Act
            var payment = Payment.CreateOutgoing(id, number, details, payer, payee, documentNr, documentDate, 
                accountingDate, amountDue, dueDate, paymentMethod, paymentCondition);

            // Assert
            Assert.AreEqual(id, payment.Id);
            Assert.AreEqual(PaymentDirection.Outgoing, payment.Direction);
            Assert.AreEqual(details, payment.Details);
            Assert.AreEqual(payer, payment.Payer);
            Assert.AreEqual(payee, payment.Payee);
            Assert.AreEqual(documentNr, payment.ReferenceNr);
            Assert.AreEqual(documentDate, payment.ReferenceDate);
            Assert.AreEqual(accountingDate, payment.AccountingDate);
            Assert.AreEqual(amountDue, payment.AmountDue);
            Assert.AreEqual(dueDate, payment.DueDate);
            Assert.AreEqual(0, payment.AmountPaid);
            Assert.AreEqual(Currency.EUR, payment.Currency);
            Assert.AreEqual(PaymentStatus.Pending, payment.Status);
            Assert.AreEqual(paymentMethod, payment.PaymentMethod);
            Assert.AreEqual(paymentCondition, payment.PaymentCondition);
        }

        [TestMethod]
        public void Update_ShouldUpdatePropertiesCorrectly()
        {
            // Arrange
            var id = new PaymentId(Guid.NewGuid());
            var number = new PaymentNumber(1234);
            var customerId = new CustomerId(Guid.NewGuid());
            var invoiceId = new InvoiceId(Guid.NewGuid());
            var payment = Payment.CreateIncoming(id, number, "Initial Details", "Initial Payer", "Initial Payee", 
                customerId, invoiceId, "12345", DateTime.Now, DateTime.Now, 100m, DateTime.Now.AddDays(30), 
                PaymentMethod.BankTransfer, PaymentCondition.Default);
            var newDetails = "Updated Details";
            var newPayer = "Updated Payer";
            var newPayee = "Updated Payee";
            var newDocumentNr = "67890";
            var newDocumentDate = DateTime.Now.AddDays(1);
            var newAmountDue = 200m;
            var newStatus = PaymentStatus.Partial;
            var newPaymentMethod = PaymentMethod.DirectDebit;
            var newPaymentCondition = PaymentCondition.Template14DaysNoDiscount;

            // Act
            var result = payment.Update(newDetails, newPayer, newPayee, newDocumentNr, newDocumentDate, 
                newAmountDue, null, newPaymentMethod, newStatus, newPaymentCondition);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(newDetails, payment.Details);
            Assert.AreEqual(newPayer, payment.Payer);
            Assert.AreEqual(newPayee, payment.Payee);
            Assert.AreEqual(newDocumentNr, payment.ReferenceNr);
            Assert.AreEqual(newDocumentDate, payment.ReferenceDate);
            Assert.AreEqual(newAmountDue, payment.AmountDue);
            Assert.IsNotNull(payment.DueDate);
            Assert.AreEqual(newStatus, payment.Status);
            Assert.AreEqual(newPaymentMethod, payment.PaymentMethod);
            Assert.AreEqual(newPaymentCondition, payment.PaymentCondition);
        }

        [TestMethod]
        public void Complete_ShouldSetStatusToPaid()
        {
            // Arrange
            var id = new PaymentId(Guid.NewGuid());
            var number = new PaymentNumber(1234);
            var customerId = new CustomerId(Guid.NewGuid());
            var invoiceId = new InvoiceId(Guid.NewGuid());
            var payment = Payment.CreateIncoming(id, number, "Details", "Payer", "Payee", customerId, invoiceId, "12345", 
                DateTime.Now, DateTime.Now, 100m, DateTime.Now.AddDays(30), PaymentMethod.Default, PaymentCondition.Default);
            var amountPaid = 100m;
            var paidAt = DateTime.Now;

            // Act
            var result = payment.Complete(amountPaid, paidAt);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(amountPaid, payment.AmountPaid);
            Assert.AreEqual(paidAt, payment.PaidAt);
            Assert.AreEqual(PaymentStatus.Paid, payment.Status);
        }
    }
}
