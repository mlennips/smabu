using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Events;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.InvoiceAggregate
{
    public class Invoice : AggregateRoot<InvoiceId>, IHasBusinessNumber<InvoiceNumber>
    {
        public override InvoiceId Id { get; }
        public CustomerId CustomerId { get; }
        public int FiscalYear { get; }
        public InvoiceNumber Number { get; private set; }
        public Address CustomerAddress { get; set; }
        public DatePeriod PerformancePeriod { get; private set; }
        public TaxRate TaxRate { get; private set; }
        public DateOnly? InvoiceDate { get; private set; }
        public bool IsReleased { get; private set; }
        public DateTime? ReleasedAt { get; private set; }
        public List<InvoiceItem> Items { get; }
        public decimal Amount => Items.Sum(x => x.TotalPrice);
        public Currency Currency { get; }
        public DateOnly SalesReportDate => DetermineSalesReportDate();

#pragma warning disable IDE0290 // Primären Konstruktor verwenden
        public Invoice(InvoiceId id, CustomerId customerId, int fiscalYear, InvoiceNumber number,
                       Address customerAddress, DatePeriod performancePeriod, bool isReleased, DateTime? releasedAt,
                       DateOnly? invoiceDate, Currency currency, TaxRate taxRate, List<InvoiceItem> items)
        {
            Id = id;
            CustomerId = customerId;
            FiscalYear = fiscalYear;
            Number = number;
            CustomerAddress = customerAddress;
            PerformancePeriod = performancePeriod;
            IsReleased = isReleased;
            ReleasedAt = releasedAt;
            InvoiceDate = invoiceDate;
            Currency = currency;
            TaxRate = taxRate;
            Items = items ?? [];
        }
#pragma warning restore IDE0290 // Primären Konstruktor verwenden

        public static Invoice Create(InvoiceId id, CustomerId customerId, int fiscalYear, Address customerAddress, DatePeriod performancePeriod, Currency currency, TaxRate taxRate)
        {
            return new Invoice(id, customerId, fiscalYear, InvoiceNumber.CreateTmp(), customerAddress, performancePeriod, false, null, null, currency, taxRate, []);
        }

        public static Invoice CreateFromTemplate(InvoiceId id, CustomerId customerId, int fiscalYear, Address mainAddress, DatePeriod performancePeriod, Invoice template)
        {
            var invoice = Create(id, customerId, fiscalYear, mainAddress, performancePeriod, template.Currency, template.TaxRate);
            foreach (var item in template.Items)
            {
                invoice.AddItem(new InvoiceItemId(Guid.NewGuid()), item.Details, item.Quantity, item.UnitPrice, item.CatalogItemId);
            }
            return invoice;
        }

        public Result Update(DatePeriod performancePeriod, TaxRate taxRate, DateOnly? invoiceDate)
        {
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult;
            }

            PerformancePeriod = performancePeriod;
            TaxRate = taxRate;
            InvoiceDate = invoiceDate;

            return Result.Success();
        }

        public override Result Delete()
        {
            var checkEditResult = CheckCanEdit();
            return checkEditResult.IsFailure ? Result.Failure(checkEditResult.Error) : Result.Success();
        }

        public Result<InvoiceItem> AddItem(InvoiceItemId id, string details, Quantity quantity, decimal unitPrice, CatalogItemId? catalogItemId = null)
        {
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult.Error;
            }

            if (string.IsNullOrWhiteSpace(details))
            {
                return InvoiceErrors.ItemDetailsEmpty;
            }

            var position = Items.OrderByDescending(x => x.Position).FirstOrDefault()?.Position + 1 ?? 1;
            var result = new InvoiceItem(id, Id, position, details, quantity, unitPrice, catalogItemId);
            Items.Add(result);
            return result;
        }

        public Result<InvoiceItem> UpdateItem(InvoiceItemId id, string details, Quantity quantity, decimal unitPrice, CatalogItemId? catalogItemId)
        {
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult.Error;
            }

            var item = Items.Find(x => x.Id == id);
            if (item == null)
            {
                return InvoiceErrors.ItemNotFound;
            }

            item.Edit(details, quantity, unitPrice, catalogItemId);
            return item;
        }

        public Result RemoveItem(InvoiceItemId id)
        {
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult.Error;
            }

            var item = Items.Find(x => x.Id == id);
            if (item == null)
            {
                return Result.Failure(new ErrorDetail("Invoice.ItemNotFound", "Item not found."));
            }

            Items.Remove(item);
            ReorderItems();
            return Result.Success();
        }

        public Result MoveItemDown(InvoiceItemId id)
        {
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult.Error;
            }

            var itemToMove = Items.Find(x => x.Id == id);
            if (itemToMove == null)
            {
                return InvoiceErrors.ItemNotFound;
            }

            var itemToMoveCurrentIdx = Items.IndexOf(itemToMove);
            if (itemToMoveCurrentIdx == Items.Count - 1)
            {
                return InvoiceErrors.ItemAlreadyAtEnd;
            }

            Items.Remove(itemToMove);
            Items.Insert(itemToMoveCurrentIdx + 1, itemToMove);
            ReorderItems();
            return Result.Success();
        }

        public Result MoveItemUp(InvoiceItemId id)
        {
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult.Error;
            }

            var itemToMove = Items.Find(x => x.Id == id);
            if (itemToMove == null)
            {
                return InvoiceErrors.ItemNotFound;
            }

            var itemToMoveCurrentIdx = Items.IndexOf(itemToMove);
            if (itemToMoveCurrentIdx == 0)
            {
                return InvoiceErrors.ItemAlreadyAtBeginning;
            }

            Items.Remove(itemToMove);
            Items.Insert(itemToMoveCurrentIdx - 1, itemToMove);
            ReorderItems();
            return Result.Success();
        }

        public Result Release(InvoiceNumber number, DateTime? releasedAt)
        { 
            var checkEditResult = CheckCanEdit();
            if (checkEditResult.IsFailure)
            {
                return checkEditResult.Error;
            }

            if (Number.IsTemporary && (number == null || number.IsTemporary))
            {
                return InvoiceErrors.NumberNotValid;
            }

            if (Number != null && !Number.IsTemporary && Number != number)
            {
                return InvoiceErrors.NumberMayNotBeChangedBelated;
            }

            if (Items.Count == 0)
            {
                return InvoiceErrors.NoPositionsToRelease;
            }

            Number = Number!.IsTemporary ? number : Number;
            ReleasedAt = releasedAt ?? DateTime.UtcNow;
            IsReleased = true;
            if (!PerformancePeriod.To.HasValue)
            {
                PerformancePeriod = DatePeriod.Create(PerformancePeriod.From.ToDateTime(TimeOnly.MinValue), DateTime.Now);
            }
            InvoiceDate ??= DateOnly.FromDateTime(ReleasedAt.Value);

            AddDomainEvent(new InvoiceReleasedEvent(Id));
            return Result.Success();
        }

        public Result WithdrawRelease()
        {
            if (!IsReleased)
            {
                return InvoiceErrors.NotReleasedYet;
            }

            IsReleased = false;
            return Result.Success();
        }

        private void ReorderItems()
        {
            var pos = 1;
            foreach (var item in Items)
            {
                item.EditPosition(pos++);
            }
        }

        private DateOnly DetermineSalesReportDate()
        {
            if (ReleasedAt != null)
            {
                return DateOnly.FromDateTime(ReleasedAt.Value);
            }
            else if (PerformancePeriod?.To.HasValue ?? false)
            {
                return PerformancePeriod.To.Value;
            }
            else if (PerformancePeriod?.From != null)
            {
                return PerformancePeriod.From;
            }
            else if (Meta != null)
            {
                return DateOnly.FromDateTime(Meta.CreatedAt);
            }
            else
            {
                return DateOnly.FromDateTime(DateTime.Now);
            }
        }

        private Result CheckCanEdit()
        {
            return IsReleased ? Result.Failure(InvoiceErrors.AlreadyReleased) : Result.Success();
        }
    }
}

