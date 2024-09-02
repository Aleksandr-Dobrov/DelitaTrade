using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Commands
{
    public class LoadDayReportCommand : CommandBase
    {
        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;

        public LoadDayReportCommand(DelitaTradeDayReport dayReport, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _dayReport.LoadDayReport(_dayReportsViewModel.LoadDayReportId);
        }
    }
}
