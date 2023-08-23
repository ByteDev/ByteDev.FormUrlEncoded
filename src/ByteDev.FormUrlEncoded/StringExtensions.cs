using System.Collections.Generic;
using System.Linq;

namespace ByteDev.FormUrlEncoded
{
    internal static class StringExtensions
    {
        public static List<string> ToList(this string source, char delimiter, bool trimValues = false)
        {
            if (string.IsNullOrEmpty(source))
                return new List<string>();

            string[] parts = source.Split(delimiter);

            if (!trimValues) 
                return parts.ToList();

            return parts
                .Select(a => a.Trim())
                .Where(s => s != string.Empty)
                .ToList();
        }
    }
}