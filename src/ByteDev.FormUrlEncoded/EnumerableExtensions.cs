using System.Collections;
using System.Text;

namespace ByteDev.FormUrlEncoded
{
    internal static class EnumerableExtensions
    {
        public static string ToCsv(this IEnumerable source)
        {
            return ToDelimitedString(source, ",");
        }

        public static string ToDelimitedString(this IEnumerable source, string delimiter)
        {
            if (source == null)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var element in source)
            {
                sb.AppendIfNotEmpty(delimiter);
                sb.Append(element);
            }

            return sb.ToString();
        }
    }
}