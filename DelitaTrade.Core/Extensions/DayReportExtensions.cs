using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Services;
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
            if (dayReport.TransmissionDate != newDayReport.TransmissionDate) dayReport.TransmissionDate = newDayReport.TransmissionDate;
        }
        public static void Update(this DayReportViewModel dayReport, DayReportViewModel newDayReport)
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
            if (dayReport.TransmissionDate != newDayReport.TransmissionDate) dayReport.TransmissionDate = newDayReport.TransmissionDate;
        }



        public static void AddInvoiceToTotals(this DayReport dayReport, InvoiceInDayReport newInvoice)
        {
            if (newInvoice.PayMethod.IsHaveAmount())
            {
                if (dayReport.Invoices.FirstOrDefault(i => i.Invoice.Number == newInvoice.Invoice.Number) == null)
                {
                    dayReport.TotalAmount += newInvoice.Invoice.Amount;
                    dayReport.TotalWeight += newInvoice.Invoice.Weight;
                }
            }

            if (newInvoice.PayMethod.IsHaveIncome())
            {
                dayReport.TotalIncome += newInvoice.Income;
            }

            if (newInvoice.PayMethod.IsHaveExpense())
            {
                dayReport.TotalExpense += Math.Abs(newInvoice.Income);
                dayReport.TotalIncome += newInvoice.Income;
            }

            if (newInvoice.PayMethod.IsOldInvoice())
            {
                dayReport.TotalOldInvoice += newInvoice.Income;
            }

            if (newInvoice.PayMethod.IsMayBeNonPay())
            {
                decimal totalIncome = 0;
                List<InvoiceInDayReport> invoices = dayReport.Invoices.Where(i => i.Invoice.Number == newInvoice.Invoice.Number).ToList();
                if (invoices.Count > 0)
                {
                    foreach (var invoice in invoices)
                    {
                        totalIncome += invoice.Income;
                    }
                    dayReport.TotalNotPay -= newInvoice.Invoice.Amount - totalIncome;
                }
                totalIncome += newInvoice.Income;
                
                dayReport.TotalNotPay += newInvoice.Invoice.Amount - totalIncome;
            }
        }
        public static void RemoveInvoiceFromTotals(this DayReport dayReport, InvoiceInDayReport deleteInvoice)
        {
            if (deleteInvoice.PayMethod.IsHaveAmount())
            {
                if (dayReport.Invoices.Where(i => i.Invoice.Number == deleteInvoice.Invoice.Number).Count() == 1)
                {
                    dayReport.TotalAmount -= deleteInvoice.Invoice.Amount;
                    dayReport.TotalWeight -= deleteInvoice.Invoice.Weight;
                }
            }

            if (deleteInvoice.PayMethod.IsHaveIncome())
            {
                dayReport.TotalIncome -= deleteInvoice.Income;
            }

            if (deleteInvoice.PayMethod.IsHaveExpense())
            {
                dayReport.TotalExpense -= Math.Abs(deleteInvoice.Income);
                dayReport.TotalIncome -= deleteInvoice.Income;
            }

            if (deleteInvoice.PayMethod.IsOldInvoice())
            {
                dayReport.TotalOldInvoice -= deleteInvoice.Income;
            }

            if (deleteInvoice.PayMethod.IsMayBeNonPay())
            {
                decimal totalIncome = 0;
                List<InvoiceInDayReport> invoices = dayReport.Invoices.Where(i => i.Invoice.Number == deleteInvoice.Invoice.Number).ToList();
                if (invoices.Count > 0)
                {
                    foreach (var invoice in invoices)
                    {                        
                        totalIncome += invoice.Income;                        
                    }
                    dayReport.TotalNotPay -= deleteInvoice.Invoice.Amount - totalIncome;
                }
                if(invoices.Count > 1)
                {
                    totalIncome -= deleteInvoice.Income;
                    dayReport.TotalNotPay += deleteInvoice.Invoice.Amount - totalIncome;
                }
                
            }
        }

        public static bool IsEnoughCash(this DayReportViewModel dayReportViewModel)
        {
            if (dayReportViewModel == null) throw new ArgumentNullException(nameof(DayReportViewModel));
            if (dayReportViewModel.TotalCash < dayReportViewModel.TotalAmount) return false;
            return true;
        }
    }
}
