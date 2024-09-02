using DelitaTrade.ViewModels;
using DelitaTrade.Models;
using System.Windows;
using System.Printing;
using System.Windows.Controls;

namespace DelitaTrade
{
    public partial class App : Application
    {
        private readonly DelitaTradeCompany _delitaTrade;

        private readonly DelitaTradeDayReport _dayReportCreator;

        public App()
        {
            _delitaTrade = new DelitaTradeCompany("Delita Trade",new XmlDataBase<DataBase>());
            _dayReportCreator = new DelitaTradeDayReport(new XmlDataBase<DayReport>());
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
