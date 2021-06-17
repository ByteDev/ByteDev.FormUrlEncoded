namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyIgnoreAndPropertyNameAttribute
    {
        public string Name { get; set; }

        [FormUrlEncodedIgnore]
        [FormUrlEncodedPropertyName("emailAddress")]
        public string Email { get; set; }
    }
}