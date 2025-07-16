namespace DelitaTrade.Common.Enums.EnumTranslators
{
    public static class PaymentTranslator
    {
        public static readonly Dictionary<DelitaLanguage, Dictionary<PaymentType, string>> CreditNoteMethodsToString = new()
        {
            [DelitaLanguage.English] = new()
            {
                [PaymentType.Cash] = "Cash",
                [PaymentType.Card] = "Card",
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                [PaymentType.Cash] = "В брой",
                [PaymentType.Card] = "С карта",
            }
        };
        public static readonly Dictionary<DelitaLanguage, Dictionary<string, PaymentType>> CreditNoteMethodsToEnum = new()
        {
            [DelitaLanguage.English] = new()
            {
                ["Cash"] = PaymentType.Cash,
                ["Card"] = PaymentType.Card,
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                ["В брой"] = PaymentType.Cash,
                ["С карта"] = PaymentType.Card,
            }
        };

        public static string GetStringValue(DelitaLanguage language, PaymentType creditNoteMethod)
        {
            return CreditNoteMethodsToString[language][creditNoteMethod];
        }

        public static PaymentType GetPayMethod(DelitaLanguage language, string creditNoteMethod)
        {
            if (CreditNoteMethodsToEnum[language].TryGetValue(creditNoteMethod, out PaymentType value)) return value;
            throw new InvalidDataException($"Incorrect value {creditNoteMethod}");
        }

        public static string GetStringValue(this PaymentType creditNoteMethod, DelitaLanguage language)
        {
            return CreditNoteMethodsToString[language][creditNoteMethod];
        }
    }
}
