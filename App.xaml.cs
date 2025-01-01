using DelitaTrade.ViewModels;
using DelitaTrade.Models;
using System.Windows;
using System.Configuration;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.SoundPlayers;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.Models.MySqlDataBase;
using DelitaTrade.Models.DI;
using Microsoft.Extensions.DependencyInjection;
using DBDelitaTrade.Common;
using DelitaReturnProtocolProvider.Services;

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
        private readonly UserService _userService;
        IServiceProvider _serviceProvider = DIContainer.BuildServiceProvider();


        public App()
        {
            _mySqlConnection = new MySqlDBConnection();
            _mySqlConnection.CreateConectionToDB();
            _mySqlDataProvider = new MySqlDBDataProvider(_mySqlConnection, new CompaniesDataBase());
            _soundService = new DelitaSoundService(new DefaultSoundPlayer(), new SoundStore([.. SoundBaseConfiguration.GetAllSounds(AppConfig)]));
            _delitaTrade = new DelitaTradeCompany("Delita Trade", _mySqlDataProvider, _serviceProvider);
            _dayReportCreator = new DelitaTradeDayReport(_soundService, _mySqlDataProvider, AppConfig);
            _userService = _serviceProvider.GetService<UserService>();
            _userService.LogIn(new UserValidationForm { LoginName = "Александр Добров", Password = "10052016.Adi" });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_delitaTrade, _dayReportCreator, _serviceProvider)
            };
            MainWindow.Show();
            base.OnStartup(e);
            _delitaTrade.LoadData();
            _delitaTrade.UpdateLoadDataBase();
            //_delitaTrade.CopyCompaniesToEF();
        }
    }
}
