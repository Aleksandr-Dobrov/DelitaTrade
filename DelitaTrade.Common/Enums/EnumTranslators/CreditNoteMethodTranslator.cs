namespace DelitaTrade.Common.Enums.EnumTranslators
{
    public static class CreditNoteMethodTranslator
    {
        public static readonly Dictionary<DelitaLanguage, Dictionary<CreditNoteMethod, string>> CreditNoteMethodsToString = new()
        {
            [DelitaLanguage.English] = new()
            {
                [CreditNoteMethod.NotDeducted] = "Not deducted",
                [CreditNoteMethod.Deducted] = "Deducted",
                [CreditNoteMethod.Bank] = "Bank"
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                [CreditNoteMethod.NotDeducted] = "Не приспаднато",
                [CreditNoteMethod.Deducted] = "Приспаднато",
                [CreditNoteMethod.Bank] = "Банка"
            }
        };
        public static readonly Dictionary<DelitaLanguage, Dictionary<string, CreditNoteMethod>> CreditNoteMethodsToEnum = new()
        {
            [DelitaLanguage.English] = new()
            {
                ["Not deducted"] = CreditNoteMethod.NotDeducted,
                ["Deducted"] = CreditNoteMethod.Deducted,
                ["Bank"] = CreditNoteMethod.Bank
            },
            [DelitaLanguage.Bulgarian] = new()
            {
                ["Не приспаднато"] = CreditNoteMethod.NotDeducted,
                ["Приспаднато"] = CreditNoteMethod.Deducted,
                ["Банка"] = CreditNoteMethod.Bank
            }
        };

        public static string GetStringValue(DelitaLanguage language, CreditNoteMethod creditNoteMethod)
        {
            return CreditNoteMethodsToString[language][creditNoteMethod];
        }

        public static CreditNoteMethod GetPayMethod(DelitaLanguage language, string creditNoteMethod)
        {
            if (CreditNoteMethodsToEnum[language].TryGetValue(creditNoteMethod, out CreditNoteMethod value)) return value;
            throw new InvalidDataException($"Incorrect value {creditNoteMethod}");
        }

        public static string GetStringValue(this CreditNoteMethod creditNoteMethod, DelitaLanguage language)
        {
            return CreditNoteMethodsToString[language][creditNoteMethod];
        }
    }
}
