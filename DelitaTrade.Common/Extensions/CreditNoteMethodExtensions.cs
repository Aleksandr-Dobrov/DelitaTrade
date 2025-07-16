using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.GlobalVariables;
using static DelitaTrade.Common.Enums.EnumTranslators.CreditNoteMethodTranslator;


namespace DelitaTrade.Common.Extensions
{
    public static class CreditNoteMethodExtensions
    {
        public static string Translate(this CreditNoteMethod payMethod)
        {
            return GetStringValue(Language, payMethod);
        }

        public static CreditNoteMethod ParseToCreditNoteMethod(this string payMethod)
        {
            return GetPayMethod(Language, payMethod);
        }
    }
}
