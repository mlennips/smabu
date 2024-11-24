using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public class Order(OrderId id, OrderNumber number, CustomerId customerId, string name, string description,
        DateOnly orderDate, DateTime? deadline, string bunchKey = "")
        : AggregateRoot<OrderId>, IHasBusinessNumber<OrderNumber>
    {
        public override OrderId Id { get; } = id;
        public override string DisplayName => $"{Number.DisplayName} {Name}";
        public OrderNumber Number { get; private set; } = number;
        public CustomerId CustomerId { get; } = customerId;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public DateOnly OrderDate { get; private set; } = orderDate;
        public DateTime? Deadline { get; private set; } = deadline;
        public string BunchKey { get; private set; } = bunchKey;
        public OrderReferences References { get; private set; } = OrderReferences.Empty;

        public static Order Create(OrderId id, OrderNumber number, CustomerId customerId, string name, DateOnly orderDate)
        {
            return customerId == null || number == null || id == null || string.IsNullOrEmpty(name)
                ? throw new ArgumentException("customerId, number, id, and name cannot be empty.")
                : new Order(id, number, customerId, name, "", orderDate, null);
        }

        public Result Update(string name, string description, DateOnly orderDate, string bunchKey, DateTime? deadline)
        {
            Name = name;
            OrderDate = orderDate;
            Description = description;
            BunchKey = bunchKey;
            Deadline = deadline;

            return Result.Success();
        }

        public Result UpdateReferences(OrderReferences references)
        {
            References = references;
            return Result.Success();
        }

        public override Result Delete()
        {
            return base.Delete();
        }
    }
}

