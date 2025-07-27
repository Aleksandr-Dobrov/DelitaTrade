using System.Reflection;

namespace DelitaTrade.Common.Utilities
{
    public static class TypeUtilities
    {
        public static IEnumerable<T?> GetAllPublicConstantValues<T>(this Type type)
        {
            return [.. type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(field => field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(T))
                .Select(field => (T?)field.GetRawConstantValue())];
        }
    }
}
