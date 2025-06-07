using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public static class ExportPayMethodCardCategories
    {
        public static Dictionary<string, int> PayMethods = new Dictionary<string, int>()
        {
            ["Банка"] = 5,
            ["Приспаднато"] = 9,
            ["Не Приспаднато"] = 11,
            ["За Анулиране"] = 9
        };
    }
}
