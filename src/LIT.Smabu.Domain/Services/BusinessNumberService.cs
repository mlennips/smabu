using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.Specifications;
using LIT.Smabu.Core;
using System.Globalization;

namespace LIT.Smabu.Domain.Services
{
    public class BusinessNumberService(IAggregateStore store)
    {
        public async Task<CustomerNumber> CreateCustomerNumberAsync()
        {
            CustomerNumber? lastNumber = await DetectLastNumberAsync<Customer, CustomerNumber>();
            return lastNumber == null ? CustomerNumber.CreateFirst() : CustomerNumber.CreateNext(lastNumber);
        }

        public async Task<InvoiceNumber> CreateInvoiceNumberAsync(InvoiceNumber current, InvoiceNumber? requested, int fiscalYear)
        {
            return current.IsTemporary
                ? requested ?? await CreateNewInvoiceNumberAsync(fiscalYear)
                : current;
        }

        public async Task<OfferNumber> CreateOfferNumberAsync()
        {
            OfferNumber? lastNumber = await DetectLastNumberAsync<Offer, OfferNumber>();
            return lastNumber == null ? OfferNumber.CreateFirst() : OfferNumber.CreateNext(lastNumber);
        }

        public async Task<OrderNumber> CreateOrderNumberAsync(int year)
        {
            OrderNumber? lastNumber = await DetectLastNumberAsync<Order, OrderNumber>(year);
            return lastNumber == null ? OrderNumber.CreateFirst(year) : OrderNumber.CreateNext(lastNumber);
        }

        public async Task<PaymentNumber> CreatePaymentNumberAsync()
        {
            PaymentNumber? lastNumber = await DetectLastNumberAsync<Payment, PaymentNumber>();
            return lastNumber == null ? PaymentNumber.CreateFirst() : PaymentNumber.CreateNext(lastNumber);
        }

        private async Task<InvoiceNumber> CreateNewInvoiceNumberAsync(int year)
        {
            InvoiceNumber? lastNumber = await DetectLastNumberAsync<Invoice, InvoiceNumber>(year);
            return lastNumber == null ? InvoiceNumber.CreateFirst(year) : InvoiceNumber.CreateNext(lastNumber);
        }

        private async Task<TBusinessNumber?> DetectLastNumberAsync<TAggregate, TBusinessNumber>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>, IHasBusinessNumber<TBusinessNumber>
            where TBusinessNumber : BusinessNumber
        {
            TAggregate? last = (await store.ApplySpecificationTask(new LastBusinessNumberSpec<TAggregate, TBusinessNumber>())).SingleOrDefault();
            TBusinessNumber? lastNumber = last?.Number;
            return lastNumber;
        }

        private async Task<TBusinessNumber?> DetectLastNumberAsync<TAggregate, TBusinessNumber>(int year)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>, IHasBusinessNumber<TBusinessNumber>
            where TBusinessNumber : BusinessNumber
        {
            TAggregate? last = (await store.ApplySpecificationTask(new LastBusinessNumberSpec<TAggregate,
                TBusinessNumber>(year))).SingleOrDefault();
            TBusinessNumber? lastNumber = last?.Number;
            return lastNumber;
        }
    }
}
