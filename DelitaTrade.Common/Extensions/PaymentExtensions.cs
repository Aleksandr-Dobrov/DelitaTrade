using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.GlobalVariables;
using static DelitaTrade.Common.Enums.EnumTranslators.PaymentTranslator;

namespace DelitaTrade.Common.Extensions
{
    public static class PaymentExtensions
    {
        public static string Translate(this PaymentType payMethod)
        {
            return GetStringValue(Language, payMethod);
        }

        public static PaymentType ParseToPayMethod(this string payMethod)
        {
            return GetPayMethod(Language, payMethod);
        }
    }
}
