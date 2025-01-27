using DelitaTrade.Core.Interfaces;
using DelitaTrade.Common.Interfaces;

namespace DelitaTrade.Core.ViewModels
{
    public class CompanyObjectViewModel : IExceptionName, INamed, ICompanyObjectIsBankPay
    {
        public const string DataSeparator = " -> ";
        public int Id { get; set; }
        public required string Name { get; set; }
        public AddressViewModel? Address { get; set; }
        public bool IsBankPay { get; set; }
        public virtual TraderViewModel? Trader { get; set; }
        public required CompanyViewModel Company { get; set; }

        public override string ToString()
        {
            return $"{Name}{DataSeparator}{Company.Name}";
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return Id == (obj as CompanyObjectViewModel)?.Id;
        }
    }
}
