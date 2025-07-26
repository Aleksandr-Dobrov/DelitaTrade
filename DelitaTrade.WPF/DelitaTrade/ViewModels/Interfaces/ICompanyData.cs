using System.ComponentModel;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface ICompanyData : INotifyPropertyChanged, IInvokablePropertyChange, IHasError
    {
        int Id { get; }
        string CompanyName { get; }
        string CompanyType { get; }
        string Bulstad {  get; }
    }
}
