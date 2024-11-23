using LIT.Smabu.Domain.CatalogAggregate.Services;
using LIT.Smabu.Domain.CustomerAggregate.Services;
using LIT.Smabu.Domain.FinancialAggregate.Services;
using LIT.Smabu.Domain.InvoiceAggregate.Services;
using LIT.Smabu.Domain.OfferAggregate.Services;
using LIT.Smabu.Domain.OrderAggregate.Services;
using LIT.Smabu.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LIT.Smabu.Domain
{
    public static class DomainServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<DeleteCustomerService>();
            services.AddScoped<DeleteInvoiceService>();
            services.AddScoped<DeleteOfferService>();
            services.AddScoped<UpdateReferencesService>();
            services.AddScoped<RemoveCatalogItemService>();
            services.AddScoped<SalesStatisticsService>();
            services.AddScoped<BusinessNumberService>();
            services.AddScoped<FinancialRelationsService>();
            return services;
        }
    }
}