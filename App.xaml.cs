﻿using DelitaTrade.ViewModels;
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


        public App()
        {
            _mySqlConnection = new MySqlDBConnection();
            _mySqlConnection.ConectToDB();
            _soundService = new DelitaSoundService(new DefaultSoundPlayer(), new SoundStore([.. SoundBaseConfiguration.GetAllSounds(AppConfig)]));
            _delitaTrade = new DelitaTradeCompany("Delita Trade",new MySqlDBDataProvider(_mySqlConnection, new CompaniesDataBase()));
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
            _delitaTrade.LoadData();
            _delitaTrade.UpdateLoadDataBase();
        }
    }
}
