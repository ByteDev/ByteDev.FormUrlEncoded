using System;
using System.Linq;
using ByteDev.Collections;
using ByteDev.FormUrlEncoded.UnitTests.TestObjects;
using ByteDev.FormUrlEncoded.UnitTests.TestObjects.AttributeObjects;
using NUnit.Framework;

namespace ByteDev.FormUrlEncoded.UnitTests
{
    [TestFixture]
    public class FormUrlEncodedSerializerDeserializeTests
    {
        [TestFixture]
        public class Deserialize
        {
            [Test]
            public void WhenDataIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentException>(() => FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(null));
            }

            [Test]
            public void WhenDataIsEmpty_ThenThrowException()
            {
                Assert.Throws<ArgumentException>(() => FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(string.Empty));
            }

            [Test]
            public void WhenOptionsIsNull_ThenThrowException()
            {
                const string data = "String=John&AnotherString=Smith";

                Assert.Throws<ArgumentNullException>(() => FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data, null));
            }

            [Test]
            public void WhenTypeHasNoProperties_ThenReturnObject()
            {
                const string data = "String=John&AnotherString=Smith";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyNoProperties>(data);

                Assert.That(result, Is.Not.Null);
            }

            [TestCase("=John")]
            [TestCase("=John&=Smith")]
            public void WhenHasNoName_AndHasValue_ThenSetPropertiesToDefault(string data)
            {
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                AssertPropertiesAreDefault(result);
            }

            [TestCase("&")]
            [TestCase("&&")]
            public void WhenIsOnlyAmpersands_ThenSetPropertiesToDefault(string data)
            {
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                AssertPropertiesAreDefault(result);
            }

            [TestCase("&String=John")]
            [TestCase("String=John&")]
            [TestCase("&String=John&")]
            public void WhenTrailingAmpersand_ThenSetProperties(string data)
            {
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                Assert.That(result.String, Is.EqualTo("John"));
                Assert.That(result.AnotherString, Is.Null);
            }

            [TestCase("NotOnType=Something")]
            [TestCase("NotOnType=")]
            [TestCase("NotOnType")]
            public void WhenTypeHasNoMatchingProperties_ThenSetPropertiesToDefault(string data)
            {
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                AssertPropertiesAreDefault(result);
            }
            
            [Test]
            public void WhenValuesAreEmptyString_ThenSetPropertiesToDefault()
            {
                const string data = "Bool=&" +
                                    "Char=&" +
                                    "Byte=&" +
                                    "Short=&" +
                                    "Int=&" +
                                    "Long=&" +
                                    "SByte=&" +
                                    "UShort=&" +
                                    "UInt=&" +
                                    "ULong=&" +
                                    "Float=&" +
                                    "Double=&" +
                                    "Decimal=" + 
                                    "Obj=" + 
                                    "String=";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyBuiltInAllTypes>(data);

                AssertPropertiesAreDefault(result);
            }

            [Test]
            public void WhenValuesNotPresent_ThenSetPropertiesToDefault()
            {
                const string data = "Bool&" +
                                    "Char&" +
                                    "Byte&" +
                                    "Short&" +
                                    "Int&" +
                                    "Long&" +
                                    "SByte&" +
                                    "UShort&" +
                                    "UInt&" +
                                    "ULong&" +
                                    "Float&" +
                                    "Double&" +
                                    "Decimal" + 
                                    "Obj" + 
                                    "String";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyBuiltInAllTypes>(data);

                AssertPropertiesAreDefault(result);
            }

            [Test]
            public void WhenTypeHasOneMatchingProperty_ThenSetPropertyValues()
            {
                const string data = "String=John";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                Assert.That(result.String, Is.EqualTo("John"));
                Assert.That(result.AnotherString, Is.Null);
            }

            [Test]
            public void WhenTypeHasTwoMatchingProperties_ThenSetPropertyValues()
            {
                const string data = "String=John&AnotherString=Smith";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                Assert.That(result.String, Is.EqualTo("John"));
                Assert.That(result.AnotherString, Is.EqualTo("Smith"));
            }

            [Test]
            public void WhenPropertyTypeIsValueType_ThenDoConvertType()
            {
                const string data = "Bool=true&" +
                                    "Char=A&" +
                                    "Byte=128&" +
                                    "Short=5&" +
                                    "Int=10&" +
                                    "Long=20&" +
                                    "SByte=1&" +
                                    "UShort=21&" +
                                    "UInt=31&" +
                                    "ULong=41&" +
                                    "Float=1.1&" +
                                    "Double=1.2&" +
                                    "Decimal=123.45";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyBuiltInValueTypes>(data);

                Assert.That(result.Bool, Is.True);
                Assert.That(result.Char, Is.EqualTo('A'));
                
                Assert.That(result.Byte, Is.EqualTo(128));
                Assert.That(result.Short, Is.EqualTo(5));
                Assert.That(result.Int, Is.EqualTo(10));
                Assert.That(result.Long, Is.EqualTo(20));

                Assert.That(result.SByte, Is.EqualTo(1));
                Assert.That(result.UShort, Is.EqualTo(21));
                Assert.That(result.UInt, Is.EqualTo(31));
                Assert.That(result.ULong, Is.EqualTo(41));

                Assert.That(result.Float, Is.EqualTo(1.1f));
                Assert.That(result.Double, Is.EqualTo(1.2d));
                Assert.That(result.Decimal, Is.EqualTo(123.45M));
            }

            [Test]
            public void WhenPropertyTypeIsObject_ThenCallToString()
            {
                const string data = "Obj=123";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyBuiltInRefTypes>(data);

                Assert.That(result.Obj, Is.EqualTo("123"));
            }
            
            [Test]
            public void WhenPropertyNameHasAtSignPrefix_ThenIgnoreAtSign()
            {
                const string data = "set=something";
                
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyAtSignPropertyName>(data);

                Assert.That(result.set, Is.EqualTo("something"));
            }

            [Test]
            public void WhenValueIsWrongType_ThenThrowException()
            {
                const string data = "Int=NotInt";

                Assert.Throws<FormatException>(() => _ = FormUrlEncodedSerializer.Deserialize<TestDummyBuiltInValueTypes>(data));
            }

            [Test]
            public void WhenSameNameAppearsTwice_ThenLastValueIsSet()
            {
                const string data = "String=John&String=Peter";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data);

                Assert.That(result.String, Is.EqualTo("Peter"));
            }

            private static void AssertPropertiesAreDefault(TestDummyTwoStringProperties result)
            {
                Assert.That(result.String, Is.EqualTo(default(string)));
                Assert.That(result.AnotherString, Is.EqualTo(default(string)));
            }

            private static void AssertPropertiesAreDefault(TestDummyBuiltInAllTypes result)
            {
                Assert.That(result.Bool, Is.False);
                Assert.That(result.Char, Is.EqualTo('\0'));
                Assert.That(result.Byte, Is.EqualTo(0));
                Assert.That(result.Short, Is.EqualTo(0));
                Assert.That(result.Int, Is.EqualTo(0));
                Assert.That(result.Long, Is.EqualTo(0));

                Assert.That(result.SByte, Is.EqualTo(0));
                Assert.That(result.UShort, Is.EqualTo(0));
                Assert.That(result.UInt, Is.EqualTo(0));
                Assert.That(result.ULong, Is.EqualTo(0));

                Assert.That(result.Float, Is.EqualTo(0));
                Assert.That(result.Double, Is.EqualTo(0));
                Assert.That(result.Decimal, Is.EqualTo(0));

                Assert.That(result.Obj, Is.Null);
                Assert.That(result.String, Is.Null);
            }
        }

        [TestFixture]
        public class Deserialize_PropertySequences
        {
            [Test]
            public void WhenPropertyIsList_AndNameNotExist_ThenSetNull()
            {
                const string data = "Name=John";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyList>(data);

                Assert.That(result.Items, Is.Null);
            }

            [Test]
            public void WhenPropertyIsList_AndValueIsEmpty_ThenSetNull()
            {
                const string data = "Items=";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyList>(data);

                Assert.That(result.Items, Is.Null);
            }

            [Test]
            public void WhenPropertyIsList_AndHasOneValue_ThenSetSequence()
            {
                const string data = "Items=John";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyList>(data);

                Assert.That(result.Items.Single(), Is.EqualTo("John"));
            }

            [Test]
            public void WhenPropertyIsList_AndHasTwoValues_ThenSetSequence()
            {
                const string data = "Items=John,Peter";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyList>(data);

                Assert.That(result.Items.Count, Is.EqualTo(2));
                Assert.That(result.Items.First(), Is.EqualTo("John"));
                Assert.That(result.Items.Second(), Is.EqualTo("Peter"));
            }

            [Test]
            public void WhenPropertyIsList_AndHasTwoSameNameValues_ThenSetSequenceToLastOnly()
            {
                const string data = "Items=John&Items=Peter";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyList>(data);

                Assert.That(result.Items.Single(), Is.EqualTo("Peter"));
            }

            [Test]
            public void WhenPropertyIsList_AndHasTwoValues_AndUsesNameAttribute_ThenSetSequence()
            {
                const string data = "list=John,Peter";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyPropertyNameAttribute>(data);

                Assert.That(result.Items.Count, Is.EqualTo(2));
                Assert.That(result.Items.First(), Is.EqualTo("John"));
                Assert.That(result.Items.Second(), Is.EqualTo("Peter"));
            }
        }

        [TestFixture]
        public class Deserialize_Options_Decode
        {
            [Test]
            public void WhenDecodeIsTrue_AndDecodePlusAsSpaceIsFalse_ThenReturnString()
            {
                const string data = "String=john%20smith";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data, new DeserializeOptions
                {
                    Decode = true,
                    DecodePlusAsSpace = false
                });

                Assert.That(result.String, Is.EqualTo("john smith"));
            }

            [Test]
            public void WhenDecodeIsTrue_AndDecodePlusAsSpaceIsTrue_ThenReturnString()
            {
                const string data = "String=john+smith";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data, new DeserializeOptions
                {
                    Decode = true,
                    DecodePlusAsSpace = true
                });

                Assert.That(result.String, Is.EqualTo("john smith"));
            }

            [Test]
            public void WhenDecodeIsFalse_ThenReturnString()
            {
                var value = "john%20smith";
                var data = "String=" + value;

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data, new DeserializeOptions
                {
                    Decode = false
                });

                Assert.That(result.String, Is.EqualTo(value));
            }

            // RFC 3986 section 2.2 Reserved Characters (January 2005)
            [TestCase("%21", "!")]
            [TestCase("%23", "#")]
            [TestCase("%24", "$")]
            [TestCase("%26", "&")]
            [TestCase("%27", "'")]
            [TestCase("%28", "(")]
            [TestCase("%29", ")")]
            [TestCase("%2A", "*")]
            [TestCase("%2B", "+")]
            [TestCase("%2C", ",")]
            [TestCase("%2F", "/")]
            [TestCase("%3A", ":")]
            [TestCase("%3B", ";")]
            [TestCase("%3D", "=")]
            [TestCase("%3F", "?")]
            [TestCase("%40", "@")]
            [TestCase("%5B", "[")]
            [TestCase("%5D", "]")]
            public void WhenValueIsReserved_ThenReturnDecodedValue(string value, string expected)
            {
                string data = "String=" + value;

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data, new DeserializeOptions
                {
                    Decode = true
                });

                Assert.That(result.String, Is.EqualTo(expected));
            }

            // RFC 3986 section 2.3 Unreserved Characters (January 2005)
            [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
            [TestCase("abcdefghijklmnopqrstuvwxyz")]
            [TestCase("0123456789")]
            [TestCase("-_.~")]
            public void WhenValueIsUnreserved_ThenReturnValue(string value)
            {
                string data = "String=" + value;

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyTwoStringProperties>(data, new DeserializeOptions
                {
                    Decode = true
                });

                Assert.That(result.String, Is.EqualTo(value));
            }
        }

        [TestFixture]
        public class Deserialize_Options_EnumType
        {
            [Test]
            public void WhenHandlingEnumByName_ThenUseName()
            {
                const string data = "TrafficLight=Yellow";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyEnumProperty>(data, new DeserializeOptions
                {
                    EnumHandling = EnumHandling.Name
                });

                Assert.That(result.TrafficLight, Is.EqualTo(TrafficLight.Yellow));
            }

            [TestCase("TrafficLight=")]
            [TestCase("TrafficLight")]
            public void WhenHandlingEnumByName_AndNotSet_ThenSetDefault(string data)
            {
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyEnumProperty>(data, new DeserializeOptions
                {
                    EnumHandling = EnumHandling.Name
                });

                Assert.That(result.TrafficLight, Is.EqualTo((TrafficLight)0));
            }

            [Test]
            public void WhenHandlingEnumByNumber_ThenUseNumber()
            {
                const string data = "TrafficLight=2";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyEnumProperty>(data, new DeserializeOptions
                {
                    EnumHandling = EnumHandling.Number
                });

                Assert.That(result.TrafficLight, Is.EqualTo(TrafficLight.Yellow));
            }

            [TestCase("TrafficLight=")]
            [TestCase("TrafficLight")]
            public void WhenHandlingEnumByNumber_AndNotSet_ThenSetDefault(string data)
            {
                var result = FormUrlEncodedSerializer.Deserialize<TestDummyEnumProperty>(data, new DeserializeOptions
                {
                    EnumHandling = EnumHandling.Number
                });

                Assert.That(result.TrafficLight, Is.EqualTo((TrafficLight)0));
            }
        }

        [TestFixture]
        public class Deserialize_PropertyNameAttribute
        {
            [Test]
            public void WhenUsesAttribute_ThenTakeNameFromAttribute()
            {
                const string data = "Name=John&emailAddress=somewhere";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyPropertyNameAttribute>(data);

                Assert.That(result.Name, Is.EqualTo("John"));
                Assert.That(result.Email, Is.EqualTo("somewhere"));
            }

            [Test]
            public void WhenAttributeNameIsNull_ThenTakeNameFromProperty()
            {
                const string data = "Email=somewhere";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyPropertyNameAttributeNull>(data);

                Assert.That(result.Email, Is.EqualTo("somewhere"));
            }

            [Test]
            public void WhenAttributeNameIsEmpty_ThenTakeNameFromProperty()
            {
                const string data = "Email=somewhere";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyPropertyNameAttributeEmpty>(data);

                Assert.That(result.Email, Is.EqualTo("somewhere"));
            }
        }

        [TestFixture]
        public class Deserialize_IgnoreAttribute
        {
            [Test]
            public void WhenIgnoreAttributeUsed_ThenIgnoreProperty()
            {
                const string data = "Name=John&Email=somewhere&Age=50";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyIgnoreAttribute>(data);

                Assert.That(result.Name, Is.EqualTo("John"));
                Assert.That(result.Email, Is.Null);
                Assert.That(result.Age, Is.EqualTo(0));
            }

            [Test]
            public void WhenPropertyHasPropertyNameAndIgnoreAttributes_ThenIgnoreProperty()
            {
                const string data = "Name=John&emailAddress=somewhere";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyIgnoreAndPropertyNameAttribute>(data);

                Assert.That(result.Name, Is.EqualTo("John"));
                Assert.That(result.Email, Is.Null);
            }
        }

        [TestFixture]
        public class Deserialze_ValueConverterAttribute
        {
            private System.Drawing.Color CorrectColor = System.Drawing.Color.LightSteelBlue;
            
            [Test]
            public void WhenUsesValueConverter_ConvertValue()
            {
                string data = $"OfficeWallColor={CorrectColor.Name}";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyValueConverterAttribute>(data);

                Assert.That(result.OfficeWallColor, Is.EqualTo(CorrectColor));
            }

            [Test]
            public void WhenUsesValueConverterAndPropertyName_ConvertValue()
            {
                string data = $"OfficeColor={CorrectColor.Name}";

                var result = FormUrlEncodedSerializer.Deserialize<TestDummyValueConverterAndNamedPropertyAttribute>(data);

                Assert.That(result.OfficeWallColor, Is.EqualTo(CorrectColor));
            }
        }
    }
}