namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyPropertyNameAttributeNull
    {
        [FormUrlEncodedPropertyName(null)]
        public string Email { get; set; }
    }
}