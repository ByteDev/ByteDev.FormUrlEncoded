using System.Drawing;

namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyColorValueConverterAttribute : FormUrlEncodedValueConverterAttribute
    {
        public override object FromString(string input)
        {
            return Color.FromName(input);
            // return int.Parse(input, System.Globalization.NumberStyles.HexNumber);
        }

        public override string ToString(object output)
        {
            var result = (Color)output;
            return result.Name;
            // var result = (int)output;
            // return result.ToString("X");
        }
    }
}
