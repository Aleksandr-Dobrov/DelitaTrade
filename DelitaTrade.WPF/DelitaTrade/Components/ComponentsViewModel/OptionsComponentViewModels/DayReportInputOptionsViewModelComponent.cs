using DelitaTrade.Models.Configurations.DayReportConfiguration;
using DelitaTrade.ViewModels;
using System.Configuration;
using System.Windows;

namespace DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels
{
    public class DayReportInputOptionsViewModelComponent : ViewModelBase
    {
		private WeightConfiguration _weightConfiguration;
		private readonly Configuration _appConfig;

        public DayReportInputOptionsViewModelComponent(Configuration appConfig)
        {
            _weightConfiguration = new WeightConfiguration();
            _appConfig = appConfig;
			SetWeightConfigurator(_appConfig);
        }
        public bool WeightIsOn
		{
			get => _weightConfiguration.IsEnabled; 
			set 
			{ 
				_weightConfiguration.IsEnabled = value;
				_weightConfiguration.CurrentConfiguration.Save();
				OnPropertyChange();
				OnPropertyChange(nameof(Visibility));
			}
		}

		public Visibility Visibility => WeightIsOn ? Visibility.Visible : Visibility.Collapsed;
		

		public void SetWeightConfigurator(Configuration appConfig)
		{
			if (appConfig.Sections["weightIsOn"] is null)
			{
				appConfig.Sections.Add("weightIsOn", _weightConfiguration);
			}

			_weightConfiguration = (WeightConfiguration)appConfig.GetSection("weightIsOn");  
			OnPropertyChange(nameof(Visibility));
        }
	}
}
