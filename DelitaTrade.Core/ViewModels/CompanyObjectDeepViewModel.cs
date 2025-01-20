using DelitaTrade.Common;

namespace DelitaTrade.Core.ViewModels
{
    public class CompanyObjectDeepViewModel : CompanyObjectViewModel
    {
        private TraderViewModel traderViewModel = new TraderViewModel
        {
            Id = DelitaDbConstants.DefaultTraderId,
            Name = DelitaDbConstants.DefaultTraderName
        };
        public override required TraderViewModel Trader
        {
            get => traderViewModel;
            set
            {
                if (value != null)
                { 
                    traderViewModel = value; 
                }
            }
        }
    }
}
