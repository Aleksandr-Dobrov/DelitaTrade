using DelitaTrade.ViewModels;
using DelitaTrade.Models;
using System.Windows;
using System.Configuration;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.SoundPlayers;
using DelitaTrade.Services;
using DelitaTrade.Stores;

namespace DelitaTrade
{
    public partial class App : Application
    {
        private readonly DelitaTradeCompany _delitaTrade;

        private readonly DelitaTradeDayReport _dayReportCreator;
        private readonly DelitaSoundService _soundService;
        private readonly Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public App()
        {
            _soundService = new DelitaSoundService(new DefaultSoundPlayer(), new SoundStore([.. SoundBaseConfiguration.GetAllSounds(AppConfig)]));
            _delitaTrade = new DelitaTradeCompany("Delita Trade",new XmlDataBase<DataBase>());
            _dayReportCreator = new DelitaTradeDayReport(new XmlDataBase<DayReport>(), _soundService);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_delitaTrade, _dayReportCreator)
            };
            MainWindow.Show();
            base.OnStartup(e);
            _delitaTrade.LoadFile();
            _delitaTrade.UpdateLoadDataBase();
        }
    }
}
