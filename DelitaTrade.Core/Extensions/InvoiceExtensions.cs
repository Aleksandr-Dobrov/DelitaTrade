using DelitaTrade.Common.Enums;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Extensions
{
    public static class InvoiceExtensions
    {
        public static void Update(this InvoiceInDayReport invoiceToUpdate, InvoiceViewModel invoice)
        {
            if (invoiceToUpdate.Invoice.Amount != invoice.Amount) invoiceToUpdate.Invoice.Amount = invoice.Amount;
            if (invoiceToUpdate.Invoice.Weight != invoice.Weight) invoiceToUpdate.Invoice.Weight = invoice.Weight;
            if (invoiceToUpdate.Invoice.CompanyId != invoice.Company.Id) invoiceToUpdate.Invoice.CompanyId = invoice.Company.Id;
            if (invoiceToUpdate.Invoice.CompanyObjectId != invoice.CompanyObject.Id) invoiceToUpdate.Invoice.CompanyObjectId = invoice.CompanyObject.Id;
            if (invoiceToUpdate.Income != invoice.Income) invoiceToUpdate.Income = invoice.Income;
            if (invoiceToUpdate.PayMethod != invoice.PayMethod) invoiceToUpdate.PayMethod = invoice.PayMethod;
        }

        public static bool IsBeUnpaid(this InvoiceViewModel invoice) 
        {
            return invoice.PayMethod == PayMethod.Cash ||
                   invoice.PayMethod == PayMethod.Card ||
                   invoice.PayMethod == PayMethod.OldPayCard ||
                   invoice.PayMethod == PayMethod.OldPayCash;
        }

        public static bool IsHaveAmount(this PayMethod payMethod)
        {
            if (payMethod == PayMethod.Cash ||
                payMethod == PayMethod.Card ||
                payMethod == PayMethod.Bank ||
                payMethod == PayMethod.ForCreditNote)
            {
                return true;
            }
            return false;
        }

        public static bool IsHaveIncome(this PayMethod payMethod)
        {
            if (payMethod == PayMethod.Cash ||
                payMethod == PayMethod.OldPayCash ||
                payMethod == PayMethod.ForCreditNote)
            {
                return true;
            }
            return false;
        }

        public static bool IsOldInvoice(this PayMethod payMethod)
        {
            if(payMethod == PayMethod.OldPayCash ||
                payMethod == PayMethod.OldPayCard)
            {
                return true;
            }
            return false;
        }

        public static bool IsHaveExpense(this PayMethod payMethod)
        {
            if (payMethod == PayMethod.Expense ||
                payMethod == PayMethod.CreditNote)
            {
                return true;
            }
            return false;
        }

        public static bool IsMayBeNonPay(this PayMethod payMethod)
        {
            if (payMethod == PayMethod.Cash ||
                payMethod == PayMethod.Cancellation ||
                payMethod == PayMethod.ForCreditNote ||
                payMethod == PayMethod.Card)
            {
                return true;
            }
            return false;
        }
    }
}
