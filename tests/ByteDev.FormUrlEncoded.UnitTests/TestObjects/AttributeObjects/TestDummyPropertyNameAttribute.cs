using System.Collections.Generic;

namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects
{
    internal class TestDummyPropertyNameAttribute
    {
        public string Name { get; set; }

        [FormUrlEncodedPropertyName("emailAddress")]
        public string Email { get; set; }

        [FormUrlEncodedPropertyName("list")]
        public List<string> Items { get; set; }
    }
}