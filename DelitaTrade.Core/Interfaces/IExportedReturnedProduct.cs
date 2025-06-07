namespace DelitaTrade.Core.Interfaces
{
    public interface IExportedReturnedProduct
    {
        string Name { get; }
        double Quantity { get; }
        string Unit { get; }
        string Batch { get; }
        string BestBefore { get; }
        string Description { get; }
        string DescriptionCategory { get; }
    }
}