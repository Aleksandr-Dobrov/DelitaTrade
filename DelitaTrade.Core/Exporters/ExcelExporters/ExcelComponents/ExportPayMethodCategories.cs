namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public static class ExportPayMethodCategories
    {
        public static Dictionary<string, int> PayMethods = new Dictionary<string, int>()
        {
            ["Банка"] = 5,
            ["Приспаднато"] = 7,
            ["Не Приспаднато"] = 11,
            ["За Анулиране"] = 7
        };
    }
}
