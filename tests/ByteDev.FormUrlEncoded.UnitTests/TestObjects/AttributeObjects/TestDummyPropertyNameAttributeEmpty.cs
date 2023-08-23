namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyPropertyNameAttributeEmpty
    {
        [FormUrlEncodedPropertyName("")]
        public string Email { get; set; }
    }
}