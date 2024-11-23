using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Domain.Base
{
    public abstract record EnumValueObject : SimpleValueObject<string>
    {
        protected EnumValueObject(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("The value must not be empty.");
            }
            if (value.Contains(' '))
            {
                throw new DomainException("The value must not contain spaces.");
            }
        }

        public abstract string Name { get; }

        public virtual string ShortName => "";
    }
}
