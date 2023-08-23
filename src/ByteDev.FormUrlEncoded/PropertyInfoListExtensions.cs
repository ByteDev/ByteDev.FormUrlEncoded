using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ByteDev.FormUrlEncoded
{
    internal static class PropertyInfoListExtensions
    {
        public static PropertyInfo GetByAttributeName(this List<PropertyInfo> source, string name)
        {
            return source.SingleOrDefault(p => p.GetAttributeName() == name);
        }
    }
}