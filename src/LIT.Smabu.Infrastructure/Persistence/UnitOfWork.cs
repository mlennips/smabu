using LIT.Smabu.Core;
using LIT.Smabu.Infrastructure.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class UnitOfWork(ILogger<CosmosAggregateRepository> logger, 
        IAggregateRepository aggregateRepository, IDomainEventDispatcher domainEventDispatcher) : IUnitOfWork
    {

    }
}
