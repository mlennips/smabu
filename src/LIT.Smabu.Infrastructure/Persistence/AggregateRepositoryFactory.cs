using LIT.Smabu.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class AggregateRepositoryFactory(IServiceProvider serviceProvider) : IAggregateRepositoryFactory
    {
        public IAggregateRepository Create()
        {
            using var scope = serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IAggregateRepository>();
        }
    }
}
