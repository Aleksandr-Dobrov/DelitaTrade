using System.Collections.ObjectModel;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using System.ComponentModel.DataAnnotations;
using DelitaTrade.Common;
using DelitaTrade.Common.Interfaces;

namespace DelitaTrade.Core.ViewModels
{ 
    public class CompanyViewModel : IExceptionName, INamed
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [MaxLength(ValidationConstants.CompanyTypeMaxLength)]
        public string? Type { get; set; }
        public string? Bulstad {  get; set; }
        public ObservableCollection<CompanyObjectViewModel> CompanyObjects { get; set; } = new ObservableCollection<CompanyObjectViewModel>();

        public override string ToString()
        {
            return Name;
        }
    }
}
