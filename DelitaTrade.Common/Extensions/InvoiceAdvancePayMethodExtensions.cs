using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.GlobalVariables;
using static DelitaTrade.Common.Enums.EnumTranslators.InvoiceAdvancePayMethodTranslator;


namespace DelitaTrade.Common.Extensions
{
    public static class InvoiceAdvancePayMethodExtensions
    {
        public static string Translate(this InvoiceAdvancePayMethods payMethod)
        {
            return GetStringValue(Language, payMethod);
        }

        public static InvoiceAdvancePayMethods ParseToPayMethod(this string payMethod)
        {
            return GetPayMethod(Language, payMethod);
        }
    }
}
