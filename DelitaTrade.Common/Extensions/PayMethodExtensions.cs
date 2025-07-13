using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.GlobalVariables;
using static DelitaTrade.Common.Enums.EnumTranslators.PayMethodTranslator;

namespace DelitaTrade.Common.Extensions
{
    public static class PayMethodExtensions
    {
        public static string Translate(this PayMethod payMethod)
        {
            return GetStringValue(Language, payMethod);
        }

        public static PayMethod ParseToPayMethod(this string payMethod) 
        {
            return GetPayMethod(Language, payMethod);
        }
    }
}
