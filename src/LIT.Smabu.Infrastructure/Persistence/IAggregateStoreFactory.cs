using LIT.Smabu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public interface IAggregateStoreFactory
    {
        IAggregateStore Create();
    }
}
