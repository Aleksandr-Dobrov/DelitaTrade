using DelitaTrade.ViewModels;
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DelitaTrade.Models.Loggers;

namespace DelitaTrade
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {                 
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(c =>
                {
                    c.AddJsonFile("delitaAppSetings.json", true)
                    .AddJsonFile("userRememberAccount.json", false)
                    .AddUserSecrets(Assembly.GetEntryAssembly() ?? throw new ArgumentNullException("Assembly not found"));                  
                })
                .ConfigureServices((hostContent, services) =>
                {
                    services.AddApplicationDatabase(hostContent.Configuration)
                        .AddApplicationConfigurationManager()
                        .AddApplicationServices()
                        .AddWpfUserServiceAndUserUi()
                        .AddWpfApplicationViewModelsAndControllers()
                        .AddApplicationIdentity()
                        .AddWpfApplicationExporterServices();
                    
                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await AppHost!.StartAsync();                
                MainWindow = new MainWindow()
                {
                    DataContext = AppHost.Services.GetRequiredService<MainViewModel>()
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
    }
}
