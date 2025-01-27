namespace DelitaTrade.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsOnlyDigits(this string value)
        {
            foreach (char c in value)
            {
                if (char.IsDigit(c) == false) return false;
            }
            return true;
        }
    }
}
