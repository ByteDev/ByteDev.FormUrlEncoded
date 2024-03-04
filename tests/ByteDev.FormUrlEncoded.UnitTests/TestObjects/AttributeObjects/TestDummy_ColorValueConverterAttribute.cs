using System.Drawing;

namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyColorValueConverterAttribute : FormUrlEncodedValueConverterAttribute
    {
        public override object ConvertFromString(string value)
        {
            return Color.FromName(value);
            // return int.Parse(input, System.Globalization.NumberStyles.HexNumber);
        }

        public override string ConvertToString(object value)
        {
            var result = (Color)value;
            return result.Name;
            // var result = (int)output;
            // return result.ToString("X");
        }
    }
}
