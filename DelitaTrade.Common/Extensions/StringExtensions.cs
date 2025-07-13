namespace DelitaTrade.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GetControllerName(this string controller)
        {
            if (string.IsNullOrEmpty(controller))
            {
                return string.Empty;
            }
            if (controller.EndsWith("Controller"))
            {
                return controller.Substring(0, controller.Length - "Controller".Length);
            }
            return controller;
        }
    }
}
