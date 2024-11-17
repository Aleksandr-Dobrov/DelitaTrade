using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Configurations.DayReportConfiguration;
using DelitaTrade.ViewModels;
using iTextSharp.text.io;
using System.Configuration;

namespace DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels
{
    public class DayReportInputOptionsViewModelComponent : ViewModelBase
    {
		private WeightConfiguration _weightConfiguration;

        public DayReportInputOptionsViewModelComponent()
        {
            _weightConfiguration = new WeightConfiguration();
        }
        public bool WeightIsOn
		{
			get => _weightConfiguration.IsEnabled; 
			set 
			{ 
				_weightConfiguration.IsEnabled = value;
				_weightConfiguration.CurrentConfiguration.Save();
				OnPropertyChange();
			}
		}

		public void SetWeightConfigurator(Configuration appConfig)
		{
			if (appConfig.Sections["weightIsOn"] is null)
			{
				appConfig.Sections.Add("weightIsOn", _weightConfiguration);
			}

			_weightConfiguration = (WeightConfiguration)appConfig.GetSection("weightIsOn");
		}
	}
}
