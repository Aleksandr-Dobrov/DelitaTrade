using System.ComponentModel;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface ICompanyData : INotifyPropertyChanged, IInvokablePropertyChange, IHasError
    {
        string CompanyName { get; }
        string CompanyType { get; }
        string Bulstad {  get; }
    }
}
