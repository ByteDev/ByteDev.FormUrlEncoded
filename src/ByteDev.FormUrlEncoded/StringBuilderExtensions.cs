using System.Text;

namespace ByteDev.FormUrlEncoded
{
    internal static class StringBuilderExtensions
    {
        public static void AppendKeyValue(this StringBuilder source, string key, string value, SerializeOptions options)
        {
            if (source.Length > 0)
                source.Append('&');

            source.Append(UrlEncoder.Encode(key, options));
            source.Append("=");
            source.Append(UrlEncoder.Encode(value, options));
        }
    }
}