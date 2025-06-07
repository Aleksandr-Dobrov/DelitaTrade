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
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using DelitaTrade.Infrastructure.Data;

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
                    services.BuildServiceCollection(hostContent.Configuration);
                    services.AddIdentity<DelitaUser, IdentityRole<Guid>>(options =>
                    {
                        options.User.RequireUniqueEmail = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    })
                    .AddEntityFrameworkStores<DelitaDbContext>()
                    .AddDefaultTokenProviders();
                    
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
