using LIT.Smabu.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class AggregateStoreFactory(IServiceProvider serviceProvider) : IAggregateStoreFactory
    {
        public IAggregateStore Create()
        {
            using var scope = serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IAggregateStore>();
        }
    }
}
