using DelitaTrade.ViewModels;
using System.Windows;
using DelitaTrade.Models.DI;
using DelitaTrade.Common;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration? _configuration;

        private IServiceProvider _serviceProvider;


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
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await AppHost!.StartAsync();
                UserLogin(_configuration);
                MainWindow = new MainWindow()
                {
                    DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
                };
                MainWindow.Show();
                base.OnStartup(e);
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
