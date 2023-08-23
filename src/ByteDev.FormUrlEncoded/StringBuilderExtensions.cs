using System;
using System.Text;

namespace ByteDev.FormUrlEncoded
{
    internal static class StringBuilderExtensions
    {
        public static void AppendKeyValue(this StringBuilder source, string key, string value, SerializeOptions options)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Length > 0)
                source.Append('&');

            source.Append(UrlEncoder.Encode(key, options));
            source.Append("=");
            source.Append(UrlEncoder.Encode(value, options));
        }

        public static void AppendKeySequenceValue(this StringBuilder source, string key, string value, SerializeOptions options)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Length > 0)
                source.Append('&');

            source.Append(UrlEncoder.Encode(key, options));
            source.Append("=");
            source.Append(UrlEncoder.Encode(value, options, false));
        }

        public static void AppendIfNotEmpty(this StringBuilder source, string value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Length > 0)
                source.Append(value);
        }
    }
}