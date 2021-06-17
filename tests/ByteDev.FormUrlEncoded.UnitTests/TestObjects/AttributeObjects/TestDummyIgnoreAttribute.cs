namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyIgnoreAttribute
    {
        public string Name { get; set; }

        [FormUrlEncodedIgnore]
        public string Email { get; set; }

        [FormUrlEncodedIgnore]
        public int Age { get; set; }
    }
}