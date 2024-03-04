namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyValueConverterAndNamedPropertyAttribute
    {
        [TestDummyColorValueConverter]
        [FormUrlEncodedPropertyName("OfficeColor")]
        public System.Drawing.Color OfficeWallColor { get; set; }
    }
}