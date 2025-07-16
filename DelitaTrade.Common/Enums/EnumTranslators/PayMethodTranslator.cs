namespace DelitaTrade.Common.Enums.EnumTranslators
{
    public static class PayMethodTranslator
    {
        public static readonly Dictionary<DelitaLanguage, Dictionary<PayMethod, string>> PayMethodsToString = new()
        {
            [DelitaLanguage.English] = new()
            {
                [PayMethod.Bank] = "Bank",
                [PayMethod.Cash] = "Cash",
                [PayMethod.Card] = "Card",
                [PayMethod.ForCreditNote] = "ForCreditNote",
                [PayMethod.Cancellation] = "Cancellation",
                [PayMethod.CreditNote] = "CreditNote",
                [PayMethod.OldPayCash] = "OldPayCash",
                [PayMethod.OldPayCard] = "OldPayCard",
                [PayMethod.Expense] = "Expense"
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                [PayMethod.Bank] = "Банка",
                [PayMethod.Cash] = "В брой",
                [PayMethod.Card] = "С карта",
                [PayMethod.ForCreditNote] = "За кредитно",
                [PayMethod.Cancellation] = "За анулиране",
                [PayMethod.CreditNote] = "Кредитно",
                [PayMethod.OldPayCash] = "Стара сметка",
                [PayMethod.OldPayCard] = "Стара с карта",
                [PayMethod.Expense] = "Разход"
            }
        };
        public static readonly Dictionary<DelitaLanguage, Dictionary<string, PayMethod>> PayMethodsToEnum = new()
        {
            [DelitaLanguage.English] = new()
            {
                ["Bank"] = PayMethod.Bank,
                ["Cash"] = PayMethod.Cash,
                ["Card"] = PayMethod.Card,
                ["ForCreditNote"] = PayMethod.ForCreditNote,
                ["Cancellation"] = PayMethod.Cancellation,
                ["CreditNote"] = PayMethod.CreditNote,
                ["OldPayCash"] = PayMethod.OldPayCash,
                ["OldPayCard"] = PayMethod.OldPayCard,
                ["Expense"] = PayMethod.Expense
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                ["Банка"] = PayMethod.Bank,
                ["В брой"] = PayMethod.Cash,
                ["С карта"] = PayMethod.Card,
                ["За кредитно"] = PayMethod.ForCreditNote,
                ["За анулиране"] = PayMethod.Cancellation,
                ["Кредитно"] = PayMethod.CreditNote,
                ["Стара сметка"] = PayMethod.OldPayCash,
                ["Стара с карта"] = PayMethod.OldPayCard,
                ["Разход"] = PayMethod.Expense
            }
        };

        public static string GetStringValue(DelitaLanguage language, PayMethod payMethod) 
        {
            return PayMethodsToString[language][payMethod];
        }

        public static PayMethod GetPayMethod(DelitaLanguage language, string payMethod)
        {
            if (PayMethodsToEnum[language].TryGetValue(payMethod, out PayMethod value)) return value;
            throw new InvalidDataException($"Incorrect value {payMethod}");
        }

        public static string GetStringValue(this PayMethod payMethod, DelitaLanguage language)
        {
            return PayMethodsToString[language][payMethod];
        }
    }
}
