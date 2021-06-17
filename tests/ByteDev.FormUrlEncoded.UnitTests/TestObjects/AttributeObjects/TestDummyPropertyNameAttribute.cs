namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyPropertyNameAttribute
    {
        public string Name { get; set; }

        [FormUrlEncodedPropertyName("emailAddress")]
        public string Email { get; set; }
    }

    internal class TestDummyPropertyNameAttributeNull
    {
        [FormUrlEncodedPropertyName(null)]
        public string Email { get; set; }
    }

    internal class TestDummyPropertyNameAttributeEmpty
    {
        [FormUrlEncodedPropertyName("")]
        public string Email { get; set; }
    }
}