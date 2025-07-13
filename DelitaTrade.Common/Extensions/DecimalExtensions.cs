namespace DelitaTrade.Common.Extensions
{
    public static class DecimalExtensions
    {
        private static readonly decimal _levToEuro = 1.95583m; 
        public static string ToLev(this decimal levValue) 
        {
            return $"{levValue:f2}лв.";
        }

        public static string ToEuro(this decimal levValue) 
        {
            return $"{levValue / _levToEuro:f2}euro";
        }
    }
}
