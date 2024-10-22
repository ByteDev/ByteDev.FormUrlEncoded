using System;

namespace ByteDev.FormUrlEncoded.ValueConverterAttributes
{
    /// <summary>
    /// <see cref="FormUrlEncodedValueConverterAttribute"/> that converts between a string represeting
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks">Ticks</see>
    /// and <see cref="DateTimeOffset"/> types.
    /// </summary>
    public class DateTimeOffsetUnixTicksValueConverter : FormUrlEncodedValueConverterAttribute
    {
        public override object ConvertFromString(string value)
        {
            var ticks = Convert.ToInt64(value);
            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }

        public override string ConvertToString(object value)
        {
            var dto = (DateTimeOffset)value;
            return dto.Ticks.ToString();
        }
    }
}