﻿using System;

namespace ByteDev.FormUrlEncoded
{
    internal static class UrlEncoder
    {
        public static string Encode(string value, SerializeOptions options, bool escapeComma = true)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (!options.Encode)
                return value;

            var escapedValue = Uri.EscapeDataString(value);

            if (!escapeComma)
            {
                escapedValue = escapedValue.Replace("%2C", ","); // Commas are valid in sequence values
            }

            if (options.EncodeSpaceAsPlus)
            {
                return escapedValue.Replace("%20", "+");
            }

            return escapedValue;
        }

        public static string Decode(string value, DeserializeOptions options)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (!options.Decode)
                return value;

            var unescapedValue = value;

            if (options.DecodePlusAsSpace)
            {
                unescapedValue = unescapedValue.Replace("+", "%20");
            }

            return Uri.UnescapeDataString(unescapedValue);
        }
    }
}