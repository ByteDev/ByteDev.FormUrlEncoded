using System.Linq;
using System.Reflection;
using ByteDev.Reflection;

namespace ByteDev.FormUrlEncoded
{
    internal static class PropertyInfoExtensions
    {
        public static string GetAttributeOrPropertyName(this PropertyInfo source)
        {
            var attribute = source.GetCustomAttributes(typeof(FormUrlEncodedPropertyNameAttribute), false).SingleOrDefault();

            if (attribute is FormUrlEncodedPropertyNameAttribute furpnAttribute)
            {
                if (!string.IsNullOrEmpty(furpnAttribute.Name))
                    return furpnAttribute.Name;
            }
                
            return source.Name;
        }

        public static string GetAttributeName(this PropertyInfo source)
        {
            var attribute = (FormUrlEncodedPropertyNameAttribute)source
                .GetCustomAttributes(typeof(FormUrlEncodedPropertyNameAttribute), false)
                .Single();

            return attribute.Name;
        }

        public static bool HasIgnoreAttribute(this PropertyInfo source)
        {
            return source.HasAttribute<FormUrlEncodedIgnoreAttribute>();
        }
    }
}