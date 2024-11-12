using LIT.Smabu.Infrastructure.Identity.Services;
using LIT.Smabu.Infrastructure.Messaging;
using LIT.Smabu.Infrastructure.Persistence;
using LIT.Smabu.Infrastructure.Reports;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.UseCases.SeedData;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LIT.Smabu.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            RegisterAggregateStore(services);
            RegisterMediatR(services);
            RegisterReportService(services, configuration);

            return services;
        }

        private static void RegisterReportService(IServiceCollection services, IConfigurationManager configuration)
        {
            IConfiguration reportConfig = configuration.GetSection("Reports");
            services.Configure<ReportsConfig>(reportConfig);
            services.AddSingleton(reportConfig.Get<ReportsConfig>()!);
            services.AddScoped<IReportFactory, QuestReportFactory>();
        }

        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var aggregateStore = scope.ServiceProvider.GetRequiredService<IAggregateStore>();

            var initialSeed = new InitialSeed(aggregateStore);
            await initialSeed.StartAsync();

            var legacyImporter = new ImportLegacyData(aggregateStore);
            await legacyImporter.StartAsync();
        }

        private static void RegisterAggregateStore(IServiceCollection services)
        {
            services.AddScoped<IAggregateStore, CosmosAggregateStore>();
        }

        private static void RegisterMediatR(IServiceCollection services)
        {
            var mediatrOpenTypes = new[]
{
              typeof(IRequestHandler<,>),
              typeof(ICommandHandler<,>),
              typeof(IQueryHandler<,>),
              typeof(INotificationHandler<>),
            };
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.ManifestModule.Name?.StartsWith("LIT.Smabu") ?? false).ToArray();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        }

    }
}
