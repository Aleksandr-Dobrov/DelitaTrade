using System.Runtime.CompilerServices;

namespace DelitaTrade.Common.Enums.EnumTranslators
{
    public static class InvoiceAdvancePayMethodTranslator
    {
        public static readonly Dictionary<DelitaLanguage, Dictionary<InvoiceAdvancePayMethods, string>> CreditNoteMethodsToString = new()
        {
            [DelitaLanguage.English] = new()
            {               
                [InvoiceAdvancePayMethods.Partial] = "Partial",
                [InvoiceAdvancePayMethods.NotPay] = "Not Pay",
                [InvoiceAdvancePayMethods.ForCreditNote] = "For Credit Note",
                [InvoiceAdvancePayMethods.Cancelation] = "Cancelation",
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                [InvoiceAdvancePayMethods.Partial] = "Частично плащане",
                [InvoiceAdvancePayMethods.NotPay] = "Не платено",
                [InvoiceAdvancePayMethods.ForCreditNote] = "За кредитно",
                [InvoiceAdvancePayMethods.Cancelation] = "За анулиране",
            }
        };
        public static readonly Dictionary<DelitaLanguage, Dictionary<string, InvoiceAdvancePayMethods>> CreditNoteMethodsToEnum = new()
        {
            [DelitaLanguage.English] = new()
            {
                ["Partial"] = InvoiceAdvancePayMethods.Partial,
                ["Not Pay"] = InvoiceAdvancePayMethods.NotPay,
                ["For Credit Note"] = InvoiceAdvancePayMethods.ForCreditNote,
                ["Cancelation"] = InvoiceAdvancePayMethods.Cancelation,
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                ["Частично плащане"] = InvoiceAdvancePayMethods.Partial,
                ["Не платено"] = InvoiceAdvancePayMethods.NotPay,
                ["За кредитно"] = InvoiceAdvancePayMethods.ForCreditNote,
                ["За анулиране"] = InvoiceAdvancePayMethods.Cancelation,
            }
        };

        public static string GetStringValue(DelitaLanguage language, InvoiceAdvancePayMethods creditNoteMethod)
        {
            return CreditNoteMethodsToString[language][creditNoteMethod];
        }

        public static InvoiceAdvancePayMethods GetPayMethod(DelitaLanguage language, string creditNoteMethod)
        {
            if (CreditNoteMethodsToEnum[language].TryGetValue(creditNoteMethod, out InvoiceAdvancePayMethods value)) return value;
            throw new InvalidDataException($"Incorrect value {creditNoteMethod}");
        }

        public static string GetStringValue(this InvoiceAdvancePayMethods creditNoteMethod, DelitaLanguage language)
        {
            return CreditNoteMethodsToString[language][creditNoteMethod];
        }
    }
}
