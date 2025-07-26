using System.Globalization;

namespace DelitaTrade.Common.Extensions
{
    public static class CompanyExtensions
    {
        private static readonly List<string> AllowedCompanyTypes =
        [
            "ООД",
            "ЕООД",
            "ЕТ",
            "АД",
            "ДЗЗД",
            "ЕАД"
        ];

        public static string? GetCompanyName(this string? companyFullName) 
        {
            if (companyFullName == null)
            {
                return null;
            }
            foreach (var type in AllowedCompanyTypes)
            {
                if (companyFullName.StartsWith($"{type} ", StringComparison.CurrentCultureIgnoreCase))
                {
                    return companyFullName.Replace($"{type} ", "", true, CultureInfo.InvariantCulture).Trim();
                }
                else if (companyFullName.EndsWith($" {type}", StringComparison.CurrentCultureIgnoreCase))
                {
                    return companyFullName.Replace($" {type}", "", true, CultureInfo.InvariantCulture).Trim();
                }
            }
            return companyFullName;
        }

        public static string GetCompanyType(this string? companyFullName) 
        {
            if (string.IsNullOrEmpty(companyFullName))
            {
                return string.Empty;
            }

            foreach (var type in AllowedCompanyTypes)
            {
                if (companyFullName.StartsWith($"{type} ", StringComparison.CurrentCultureIgnoreCase)
                    || companyFullName.EndsWith($" {type}", StringComparison.CurrentCultureIgnoreCase))
                {
                    return type.Trim();
                }
            }

            return "NoType";
        }
    }
}
