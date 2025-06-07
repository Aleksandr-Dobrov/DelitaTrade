using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public enum ExcelReturnCategoryType
    {
        WrongOrder = 1,
        Cancellate,
        NotLoaded,
        ShortExpired,
        WithdrawalOfGoods,
        TornPackage,
        VacuumFail,
        WrongQuantity,
        BadQuality,
        WithoutLabel
    }

    public static class ExcelReturnCategory
    {
        public static Dictionary<ExcelReturnCategoryType, string> Categories = new Dictionary<ExcelReturnCategoryType, string>()
        {
            [ExcelReturnCategoryType.WrongOrder] = "ГРЕШНА ЗАЯВКА",
            [ExcelReturnCategoryType.Cancellate] = "ОТКАЗАНА ПОРЪЧКА",
            [ExcelReturnCategoryType.NotLoaded] = "НЕНАТОВАРЕНА СТОКА",
            [ExcelReturnCategoryType.ShortExpired] = "КЪС СРОК НА ГОДНОСТ",
            [ExcelReturnCategoryType.WithdrawalOfGoods] = "ИЗТЕГЛЯНЕ НА СТОКА",
            [ExcelReturnCategoryType.TornPackage] = "СКЪСАНА ОПАКОВКА",
            [ExcelReturnCategoryType.VacuumFail] = "РАЗВАКУУМИРАНО",
            [ExcelReturnCategoryType.WrongQuantity] = "ГРЕШНО КОЛИЧЕСТВО",
            [ExcelReturnCategoryType.BadQuality] = "ЛОШО КАЧЕСТВО",
            [ExcelReturnCategoryType.WithoutLabel] = "БЕЗ ЕТИКЕТИРОВКА"
        };
    }
}
