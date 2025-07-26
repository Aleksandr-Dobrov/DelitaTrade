using System.Globalization;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace DelitaTrade.Extensions
{
    public static class DataParseExtensions
    {
        public static Address? AddressParse(this string arg)
        {
            if (arg.IsNullOrEmpty()) return null;

            string? city = TryGetCity(arg);
            if (city == null) return null;
            arg = arg.Replace(city, string.Empty, true, culture: CultureInfo.InvariantCulture).Trim();

            var address = new Address { Town = city };
            string gpsCoordinate;
            if (arg.Contains('(') && arg.Contains(')'))
            {
                int startIndex = arg.IndexOf('(') + 1;
                int endIndex = arg.IndexOf(')');
                gpsCoordinate = arg[startIndex..endIndex];
                arg = arg.Replace($"({gpsCoordinate})", string.Empty, true, culture: CultureInfo.InvariantCulture);
                address.GpsCoordinates = gpsCoordinate;
            }
            string[]? streetNumber = TryGetStreet(arg)?.Split("-=-");

            if (streetNumber != null && streetNumber.Length == 1)
            {
                address.StreetName = streetNumber[0];
            }
            else if(streetNumber != null)
            {
                address.StreetName = streetNumber[0];
                address.Number = streetNumber[1];
            }

            if (streetNumber != null)
            {
                foreach (var argument in streetNumber)
                {
                    arg = arg.Replace(argument, string.Empty, true, culture: CultureInfo.InvariantCulture).Trim();
                }
            }

            if (arg.Length > 0)
            {
                address.Description = arg.Trim();
            }

            return address;
        }

        private static string? TryGetCity(string arg)
        {
            string[] cities =
            [
                "Ахелой",
                "Банкя",
                "Банско",
                "Благоевград",
                "Белчински бани",
                "Божурище",
                "Боровец",
                "Ботевград",
                "Бургас",
                "Бусманци",
                "Вакарел",
                "Варна",
                "Велико Търново",
                "Велинград",
                "Гоце Делчев",
                "Добрич",
                "Долни Пасарел",
                "Дупница",
                "Казанлък",
                "Казичене",
                "Карлово",
                "Кладница",
                "Кубратово",
                "Кюстендил",
                "Лозен",
                "Луковит",
                "Мрамор",
                "Негован",
                "Несебър",
                "Пазарджик",
                "Панагюрище",
                "Панчарево",
                "Перник",
                "Пловдив",
                "Попово",
                "Равно Поле",
                "Радомир",
                "Разлог",
                "Рударци",
                "Русе",
                "Самоков",
                "Свиленград",
                "Сливен",
                "Слънчев бряг",
                "Смолян",
                "Стара Загора",
                "Студена",
                "София",
                "Търговище",
                "Хасково",
                "Челопеч",
                "Черноморец",
                "Шумен",

            ];
            foreach (var city in cities)
            {
                if (arg.Contains(city, StringComparison.CurrentCultureIgnoreCase)) return city;
            }
            return null;
        }

        private static string? TryGetStreet(string arg)
        {
            string[] streetMarks = ["бул","ул"];
            foreach (var streetMark in streetMarks)
            {
                if (arg.StartsWith(streetMark) || arg.Contains($" {streetMark}"))
                {
                    int startIndex = -1;
                    if (arg.Contains($"{streetMark} "))
                    {
                        startIndex = arg.IndexOf($"{streetMark} ");
                    }
                    else if (arg.Contains($"{streetMark}."))
                    {
                        startIndex = arg.IndexOf($"{streetMark}.");
                    }

                    if (startIndex == -1) continue;
                    int endStreetIndex = -1;
                    int endNumberIndex = -1;
                    bool isNumber = false;
                    for (int i = startIndex + streetMark.Length; i < arg.Length; i++)
                    {
                        if (char.IsDigit(arg[i]) && isNumber == false) 
                        {
                            isNumber = true;
                            endStreetIndex = i;
                        }
                        else if (isNumber == true)
                        {
                            if (arg[i] == ' ')
                            {
                                endNumberIndex = i;
                                break;
                            }
                        }
                    }

                    if (endStreetIndex == -1)
                    {
                        return arg;
                    }

                    string street;
                    string number;

                    if (endNumberIndex == -1)
                    {
                        street = arg.Substring(startIndex, endStreetIndex - startIndex).Trim();
                        number = arg.Substring(endStreetIndex, arg.Length - endStreetIndex);
                    }
                    else
                    {
                        street = arg.Substring(startIndex, endStreetIndex - startIndex).Trim();
                        number = arg.Substring(endStreetIndex, endNumberIndex - endStreetIndex);                        
                    }
                    return $"{street}-=-{number}";
                }
            }

            return null;
        }
    }
}
