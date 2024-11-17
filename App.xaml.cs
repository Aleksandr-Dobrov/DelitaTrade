using DelitaTrade.ViewModels;
using DelitaTrade.Models;
using System.Windows;
using System.Configuration;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.SoundPlayers;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade
{
    public partial class App : Application
    {
        private readonly DelitaTradeCompany _delitaTrade;

        private readonly DelitaTradeDayReport _dayReportCreator;
        private readonly DelitaSoundService _soundService;
        private readonly Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private readonly MySqlDBConnection _mySqlConnection;
        private readonly MySqlDBDataProvider _mySqlDataProvider;


        public App()
        {
            _mySqlConnection = new MySqlDBConnection();
            _mySqlConnection.CreateConectionToDB();
            _mySqlDataProvider = new MySqlDBDataProvider(_mySqlConnection, new CompaniesDataBase());
            _soundService = new DelitaSoundService(new DefaultSoundPlayer(), new SoundStore([.. SoundBaseConfiguration.GetAllSounds(AppConfig)]));
            _delitaTrade = new DelitaTradeCompany("Delita Trade", _mySqlDataProvider);
            _dayReportCreator = new DelitaTradeDayReport(_soundService, _mySqlDataProvider, AppConfig);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_delitaTrade, _dayReportCreator)
            };
            MainWindow.Show();
            base.OnStartup(e);
            _delitaTrade.LoadData();
            _delitaTrade.UpdateLoadDataBase();
        }
    }
}
