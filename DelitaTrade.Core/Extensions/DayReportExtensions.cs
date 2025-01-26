using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Extensions
{
    public static class DayReportExtensions
    {
        public static void Update(this DayReport dayReport, DayReportViewModel newDayReport)
        {
            if (dayReport.Date != newDayReport.Date) dayReport.Date = newDayReport.Date;
            if (dayReport.Banknotes != newDayReport.Banknotes) dayReport.Banknotes = newDayReport.Banknotes;
            if (dayReport.TotalAmount != newDayReport.TotalAmount) dayReport.TotalAmount = newDayReport.TotalAmount;
            if (dayReport.TotalCash != newDayReport.TotalCash) dayReport.TotalCash = newDayReport.TotalCash;
            if (dayReport.TotalNotPay != newDayReport.TotalNotPay) dayReport.TotalNotPay = newDayReport.TotalNotPay;
            if (dayReport.TotalIncome != newDayReport.TotalIncome) dayReport.TotalIncome = newDayReport.TotalIncome;
            if (dayReport.TotalOldInvoice != newDayReport.TotalOldInvoice) dayReport.TotalOldInvoice = newDayReport.TotalOldInvoice;
            if (dayReport.TotalExpense != newDayReport.TotalExpense) dayReport.TotalExpense = newDayReport.TotalExpense;
            if (dayReport.TotalWeight != newDayReport.TotalWeight) dayReport.TotalWeight = newDayReport.TotalWeight;
            if (dayReport.VehicleId != newDayReport.Vehicle?.Id) dayReport.VehicleId = newDayReport.Vehicle?.Id;
        }
    }
}
