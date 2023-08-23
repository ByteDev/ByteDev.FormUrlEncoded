using System;
using System.Collections.Generic;
using ByteDev.FormUrlEncoded.UnitTests.TestObjects;
using ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects;
using NUnit.Framework;

namespace ByteDev.FormUrlEncoded.UnitTests
{
    [TestFixture]
    public class FormUrlEncodedSerializerSerializeTests
    {
        [TestFixture]
        public class Serialize
        {
            [Test]
            public void WhenObjectIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FormUrlEncodedSerializer.Serialize(null));
            }

            [Test]
            public void WhenOptionsIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FormUrlEncodedSerializer.Serialize(new TestDummyTwoStringProperties(), null));
            }

            [Test]
            public void WhenObjectHasNoProperties_ThenReturnEmpty()
            {
                var obj = new TestDummyNoProperties();

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenObjectHasOnlyFields_ThenReturnEmpty()
            {
                var obj = new TestDummyPublicField
                {
                    PublicString = "Public",
                    InternalString = "Internal"
                };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenPropertiesAreNull_ThenReturnEmpty()
            {
                var obj = new TestDummyTwoStringProperties { String = null, AnotherString = null };
                
                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenNonStringNotSet_ThenReturnDefaults()
            {
                var obj = new TestDummyBuiltInValueTypes();
                
                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Bool=False&Char=%00&Byte=0&Short=0&Int=0&Long=0&SByte=0&UShort=0&UInt=0&ULong=0&Float=0&Double=0&Decimal=0"));
            }

            [Test]
            public void WhenNonStringSet_ThenReturnSerializedString()
            {
                var obj = new TestDummyBuiltInValueTypes
                {
                    Bool = true,
                    Char = 'A',
                    Byte = 1,
                    Short = 10,
                    Int = 100,
                    Long = 1000,
                    SByte = 2,
                    UShort = 20,
                    UInt = 200,
                    ULong = 2000,
                    Float = 0.1f,
                    Double = 0.02,
                    Decimal = 1.003M
                };
                
                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Bool=True&Char=A&Byte=1&Short=10&Int=100&Long=1000&SByte=2&UShort=20&UInt=200&ULong=2000&Float=0.1&Double=0.02&Decimal=1.003"));
            }

            [Test]
            public void WhenPropertiesAreEmpty_ThenReturnSerializedString()
            {
                var obj = new TestDummyTwoStringProperties { String = string.Empty, AnotherString = string.Empty };
                
                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("String=&AnotherString="));
            }

            [Test]
            public void WhenMultiplePropertiesSet_ThenReturnSerializedString()
            {
                var obj = new TestDummyTwoStringProperties
                {
                    String = "TestString",
                    AnotherString = "AnotherTestString"
                };

                string expected = $"String=TestString&AnotherString=AnotherTestString";

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo(expected));
            }

            [Test]
            public void WhenValueHasSpaces_ThenEscapeToPlus()
            {
                var obj = new TestDummyTwoStringProperties { String = "john smith" };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("String=john+smith"));
            }

            [Test]
            public void WhenPropertyNameHasAtSignPrefix_ThenIgnoreAtSign()
            {
                var obj = new TestDummyAtSignPropertyName { set = "something" };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("set=something"));
            }

            [Test]
            public void WhenPropertyTypeIsCustomClass_ThenCallToString()
            {
                var obj = new TestDummyCustomObjectProperty { MyClass = new MyClass() };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("MyClass=ThisIsMyClass"));
            }

            [Test]
            public void WhenPropertyTypeIsCustomStruct_ThenCallToString()
            {
                var obj = new TestDummyStructProperty { MyStruct = new TestStruct(1) };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("MyStruct=ThisIsMyStruct"));
            }
        }

        [TestFixture]
        public class Serialize_PropertySequences
        {
            [Test]
            public void WhenPropertyIsEnumerable_AndIsNull_ThenReturnEmpty()
            {
                var obj = new TestDummyEnumerable { Items = null };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenPropertyIsEnumerable_AndIsEmpty_ThenReturnEmptyValue()
            {
                var obj = new TestDummyEnumerable { Items = new List<string>() };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Items="));
            }

            [Test]
            public void WhenPropertyIsEnumerable_AndHasSingleValues_ThenReturnSerializedString()
            {
                var obj = new TestDummyEnumerable { Items = new[] { "John" } };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Items=John"));
            }

            [Test]
            public void WhenPropertyIsEnumerable_AndHasTwoValues_ThenReturnSerializedString()
            {
                var obj = new TestDummyEnumerable { Items = new[] { "John", "Peter" } };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Items=John,Peter"));
            }
        }

        [TestFixture]
        public class Serialize_Options_Encode : FormUrlEncodedSerializerSerializeTests
        {
            [Test]
            public void WhenEncodeIsTrue_AndEncodeSpaceAsPlusIsFalse_ThenReturnString()
            {
                var obj = new TestDummyTwoStringProperties { String = "john smith" };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    Encode = true,
                    EncodeSpaceAsPlus = false
                });

                Assert.That(result, Is.EqualTo("String=john%20smith"));
            }

            [Test]
            public void WhenEncodeIsTrue_AndEncodeSpaceAsPlusIsTrue_ThenReturnString()
            {
                var obj = new TestDummyTwoStringProperties { String = "john smith" };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    Encode = true,
                    EncodeSpaceAsPlus = true
                });

                Assert.That(result, Is.EqualTo("String=john+smith"));
            }

            [Test]
            public void WhenEncodeIsFalse_ThenReturnString()
            {
                var obj = new TestDummyTwoStringProperties { String = "john smith" };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    Encode = false
                });

                Assert.That(result, Is.EqualTo("String=john smith"));
            }

            // RFC 3986 section 2.2 Reserved Characters (January 2005)
            [TestCase("!", "%21")]
            [TestCase("#", "%23")]
            [TestCase("$", "%24")]
            [TestCase("&", "%26")]
            [TestCase("'", "%27")]
            [TestCase("(", "%28")]
            [TestCase(")", "%29")]
            [TestCase("*", "%2A")]
            [TestCase("+", "%2B")]
            [TestCase(",", "%2C")]
            [TestCase("/", "%2F")]
            [TestCase(":", "%3A")]
            [TestCase(";", "%3B")]
            [TestCase("=", "%3D")]
            [TestCase("?", "%3F")]
            [TestCase("@", "%40")]
            [TestCase("[", "%5B")]
            [TestCase("]", "%5D")]
            public void WhenValueIsReserved_ThenEncodeValue(string value, string expected)
            {
                var obj = new TestDummyTwoStringProperties { String = value };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    Encode = true,
                    EncodeSpaceAsPlus = false
                });

                Assert.That(result, Is.EqualTo("String=" + expected));
            }

            // RFC 3986 section 2.3 Unreserved Characters (January 2005)
            [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
            [TestCase("abcdefghijklmnopqrstuvwxyz")]
            [TestCase("0123456789")]
            [TestCase("-_.~")]
            public void WhenValueIsUnreserved_ThenDoNotEncodeValue(string value)
            {
                var obj = new TestDummyTwoStringProperties { String = value };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    Encode = true
                });

                Assert.That(result, Is.EqualTo("String=" + value));
            }
        }

        [TestFixture]
        public class Serialize_Options_IgnoreIfNull : FormUrlEncodedSerializerSerializeTests
        {
            [Test]
            public void WhenIgnoreIfNullIsTrue_ThenDoNotSerializeNullProperties()
            {
                var obj = new TestDummyTwoStringProperties { String = null, AnotherString = "something" };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    IgnoreIfNull = true
                });

                Assert.That(result, Is.EqualTo("AnotherString=something"));
            }

            [Test]
            public void WhenIgnoreIfNullIsFalse_ThenSetNullPropertiesToEmpty()
            {
                var obj = new TestDummyTwoStringProperties { String = null, AnotherString = "something" };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    IgnoreIfNull = false
                });

                Assert.That(result, Is.EqualTo("String=&AnotherString=something"));
            }
        }

        [TestFixture]
        public class Serialize_Options_IgnoreIfDefault : FormUrlEncodedSerializerSerializeTests
        {
            [Test]
            public void WhenIgnoreIfDefaultIsTrue_ThenDoNotSerializeDefaultProperties()
            {
                var obj = new TestDummyBuiltInAllTypes();
                
                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    IgnoreIfDefault = true
                });

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenIgnoreIfDefaultIsTrue_AndIgnoreIfNullIsFalse_ThenDoNotSerializeDefaultProperties()
            {
                var obj = new TestDummyBuiltInAllTypes();
                
                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    IgnoreIfNull = false,
                    IgnoreIfDefault = true
                });

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class Serialize_Options_EnumType : FormUrlEncodedSerializerSerializeTests
        {
            [Test]
            public void WhenHandlingEnumByName_ThenUseName()
            {
                var obj = new TestDummyEnumProperty { TrafficLight = TrafficLight.Yellow };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    EnumHandling = EnumHandling.Name
                });

                Assert.That(result, Is.EqualTo("TrafficLight=Yellow"));
            }

            [Test]
            public void WhenHandlingEnumByName_AndNotSet_ThenSetZero()
            {
                var obj = new TestDummyEnumProperty();

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    EnumHandling = EnumHandling.Name
                });

                Assert.That(result, Is.EqualTo("TrafficLight=0"));
            }

            [Test]
            public void WhenHandlingEnumByNumber_ThenUseNumber()
            {
                var obj = new TestDummyEnumProperty { TrafficLight = TrafficLight.Yellow };

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    EnumHandling = EnumHandling.Number
                });

                Assert.That(result, Is.EqualTo("TrafficLight=2"));
            }

            [Test]
            public void WhenHandlingEnumByNumber_AndNotSet_ThenSetDefault()
            {
                var obj = new TestDummyEnumProperty();

                var result = FormUrlEncodedSerializer.Serialize(obj, new SerializeOptions
                {
                    EnumHandling = EnumHandling.Number
                });

                Assert.That(result, Is.EqualTo("TrafficLight=0"));
            }
        }

        [TestFixture]
        public class Serialize_PropertyNameAttribute
        {
            [Test]
            public void WhenAttributeNameSpecified_ThenTakeNameFromAttribute()
            {
                var obj = new TestDummyPropertyNameAttribute { Name = "John", Email = "john@somewhere.com" };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Name=John&emailAddress=john%40somewhere.com"));
            }

            [Test]
            public void WhenAttributeNameIsNull_ThenTakeNameFromProperty()
            {
                var obj = new TestDummyPropertyNameAttributeNull { Email = "somewhere" };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Email=somewhere"));
            }

            [Test]
            public void WhenAttributeNameIsEmpty_ThenTakeNameFromProperty()
            {
                var obj = new TestDummyPropertyNameAttributeEmpty { Email = "somewhere" };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Email=somewhere"));
            }
        }

        [TestFixture]
        public class Serialize_IgnoreAttribute : FormUrlEncodedSerializerSerializeTests
        {
            [Test]
            public void WhenIgnoreAttributeUsed_ThenIgnoreProperty()
            {
                var obj = new TestDummyIgnoreAttribute
                {
                    Name = "John",
                    Email = "john@somewhere.com",
                    Age = 50
                };

                var result = FormUrlEncodedSerializer.Serialize(obj);

                Assert.That(result, Is.EqualTo("Name=John"));
            }
        }
    }
}