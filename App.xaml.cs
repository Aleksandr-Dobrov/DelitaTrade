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
using DelitaTrade.Common;
using Microsoft.Extensions.Configuration;
using DelitaTrade.Extensions;
using DelitaTrade.ViewModels.Controllers;
using System.Reflection;
using DelitaTrade.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DelitaTrade.Models.Loggers;

namespace DelitaTrade
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        private IConfiguration _configuration;

        private readonly DelitaTradeCompany _delitaTrade;

        private readonly DelitaTradeDayReport _dayReportCreator;
        private readonly DelitaSoundService _soundService;
        private readonly Configuration AppConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private readonly MySqlDBConnection _mySqlConnection;
        private readonly MySqlDBDataProvider _mySqlDataProvider;
        IServiceProvider _serviceProvider;


        public App()
        {                 
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(c =>
                {
                    c.AddJsonFile("delitaAppSetings", true)
                    .AddUserSecrets(Assembly.GetEntryAssembly());
                })
                .ConfigureServices((hostContent, services) =>
                {
                    services.BuildServiceCollection(hostContent.Configuration);
                    _configuration = hostContent.Configuration;
                }).Build();
            _serviceProvider = AppHost.Services;
            _mySqlConnection = new MySqlDBConnection();
            _mySqlConnection.CreateConnectionToDB(_configuration);
            _mySqlDataProvider = new MySqlDBDataProvider(_mySqlConnection, new CompaniesDataBase(), _configuration);
            _soundService = new DelitaSoundService(new DefaultSoundPlayer(), new SoundStore([.. SoundBaseConfiguration.GetAllSounds(AppConfig)]));
            _delitaTrade = new DelitaTradeCompany("Delita Trade", _mySqlDataProvider, _serviceProvider);
            _dayReportCreator = new DelitaTradeDayReport(_soundService, _mySqlDataProvider, AppConfig, _configuration);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await AppHost!.StartAsync();
                UserLogin(_configuration);
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
            catch (Exception ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error).Log("Something went wrong", Logger.LogLevel.Error);
                await AppHost!.StopAsync();
                AppHost.Dispose();
                Shutdown();
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }

        private async Task UserConfig(IConfiguration config)
        {
            var section = config.GetSection("MyUserAccount");
            var userName = section.GetValue(typeof(string), "Login") as string;
            var password = section.GetValue(typeof(string), "Password") as string;
            if (userName == null || password == null) throw new ArgumentNullException(nameof(userName));
            using var scope = _serviceProvider.CreateScope();
            IUserService userService = scope.GetService<IUserService>();
            //await userService.CreateUser(new UserValidationForm { LoginName = userName, Password = password });
            var user = await userService.LogIn(new UserValidationForm { LoginName = userName, Password = password});
            scope.GetService<UserController>().LogIn(user);
        }

        private async void UserLogin(IConfiguration config)
        {
            await UserConfig(config);
        }
    }
}
