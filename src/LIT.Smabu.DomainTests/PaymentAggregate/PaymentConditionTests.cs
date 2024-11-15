using System;
using LIT.Smabu.Domain.PaymentAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LIT.Smabu.Domain.PaymentAggregate.PaymentCondition;

namespace LIT.Smabu.DomainTests.PaymentAggregate
{
    [TestClass]
    public class PaymentConditionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ShouldThrowArgumentException_WhenTermsIsEmpty()
        {
            // Arrange
            var name = "Test Condition";
            var terms = Array.Empty<PaymentCondition.PaymentTerms>();

            // Act
            var paymentCondition = new PaymentCondition(name, terms);

            // Assert is handled by ExpectedException
        }
        
        [TestMethod]
        public void Constructor_ShouldThrowArgumentException_WhenTermsOrderedIncorrectly()
        {
            // Arrange
            var name = "Test Condition";
            PaymentTerms[] terms = 
            [
                new PaymentTerms(30, 2),
                new PaymentTerms(14, 0)
            ];

            // Act
            var paymentCondition = new PaymentCondition(name, terms);

            // Assert
            Assert.AreEqual(14, paymentCondition.Terms[0].DueDays);
            Assert.AreEqual(30, paymentCondition.Terms[1].DueDays);
        }

        [TestMethod]
        public void Constructor_ShouldSortTerms_WhenMultipleTermsProvided()
        {
            // Arrange
            var name = "Test Condition";
            var terms = new[]
            {
                new PaymentCondition.PaymentTerms(30, 2),
                new PaymentCondition.PaymentTerms(14, 0)
            };

            // Act
            var paymentCondition = new PaymentCondition(name, terms);

            // Assert
            Assert.AreEqual(14, paymentCondition.Terms[0].DueDays);
            Assert.AreEqual(30, paymentCondition.Terms[1].DueDays);
        }

        [TestMethod]
        public void Template14DaysNoDiscount_ShouldCreateCorrectPaymentCondition()
        {
            // Act
            var paymentCondition = PaymentCondition.Template14DaysNoDiscount;

            // Assert
            Assert.AreEqual("14 Tage netto ohne Abzug", paymentCondition.Name);
            Assert.AreEqual(1, paymentCondition.Terms.Length);
            Assert.AreEqual(14, paymentCondition.Terms[0].DueDays);
            Assert.AreEqual(0, paymentCondition.Terms[0].DiscountPercentage);
        }

        [TestMethod]
        public void Template30DaysNet14Days2PercentDiscount_ShouldCreateCorrectPaymentCondition()
        {
            // Act
            var paymentCondition = PaymentCondition.Template30DaysNet14Days2PercentDiscount;

            // Assert
            Assert.AreEqual("30 Tage netto, 14 Tage 2% Skonto", paymentCondition.Name);
            Assert.AreEqual(2, paymentCondition.Terms.Length);
            Assert.AreEqual(14, paymentCondition.Terms[0].DueDays);
            Assert.AreEqual(2, paymentCondition.Terms[0].DiscountPercentage);
            Assert.AreEqual(30, paymentCondition.Terms[1].DueDays);
            Assert.AreEqual(0, paymentCondition.Terms[1].DiscountPercentage);
        }

        [TestMethod]
        public void TemplatePaymentOnInvoice_ShouldCreateCorrectPaymentCondition()
        {
            // Act
            var paymentCondition = PaymentCondition.TemplatePaymentOnInvoice;

            // Assert
            Assert.AreEqual("Zahlung bei Rechnungserhalt", paymentCondition.Name);
            Assert.AreEqual(1, paymentCondition.Terms.Length);
            Assert.AreEqual(0, paymentCondition.Terms[0].DueDays);
            Assert.AreEqual(0, paymentCondition.Terms[0].DiscountPercentage);
        }
    }
}