using System;
using System.Globalization;

namespace ByteDev.FormUrlEncoded.ValueConverterAttributes
{
    /// <summary>
    /// <see cref="FormUrlEncodedValueConverterAttribute"/> that converts between an 
    /// <see href="https://www.iso.org/iso-8601-date-and-time-format.html">ISO8601</see>-formatted string 
    /// and <see cref="DateTimeOffset"/> types.
    /// </summary>
    public class DateTimeOffsetIso8601ValueConverter : FormUrlEncodedValueConverterAttribute
    {
        public override object ConvertFromString(string value)
        {
            DateTimeOffset.TryParse(value, out DateTimeOffset result);
            return result;
        }

        public override string ConvertToString(object value)
        {
            var dto = (DateTimeOffset)value;
            var str = dto.ToString("o", CultureInfo.InvariantCulture);
            return str;
        }
    }
}