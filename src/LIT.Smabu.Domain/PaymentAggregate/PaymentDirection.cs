﻿using LIT.Smabu.Domain.Shared;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public record PaymentDirection: SimpleValueObject<string>
    {
        private const string IncomingKey = "Incoming";
        private const string OutgoingKey = "Outgoing";

        public PaymentDirection(string value) : base(value)
        {
            if (value != IncomingKey && value != OutgoingKey)
            {
                throw new ArgumentException("Invalid payment direction value.", nameof(value));
            }
        }

        public static PaymentDirection Incoming { get; } = new PaymentDirection(IncomingKey);
        public static PaymentDirection Outgoing { get; } = new PaymentDirection(OutgoingKey);
    }
}
