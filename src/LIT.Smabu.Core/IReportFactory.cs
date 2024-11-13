namespace LIT.Smabu.Core
{
    public interface IReportFactory
    {
        Task<IReport> CreateInvoiceReportAsync(IEntityId id);
        Task<IReport> CreateOfferReportAsync(IEntityId id);
    }
}
