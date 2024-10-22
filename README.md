[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.FormUrlEncoded?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-FormUrlEncoded/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.FormUrlEncoded.svg)](https://www.nuget.org/packages/ByteDev.FormUrlEncoded)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/ByteDev/ByteDev.FormUrlEncoded/blob/master/LICENSE)

# ByteDev.FormUrlEncoded

.NET Standard library for serializing and deserializing form URL encoded (`application/x-www-form-urlencoded`) data.

The form URL encoded format is a data format that encodes data as name-value pairs. It is most associated with web HTML forms and the visible web. However, it can also be used in other situations, for example as a form of data exchange by APIs. This library allows a consumer to serailize/deserialize data in the format without being tied to a web application environment such as ASP.NET.

## Installation

ByteDev.FormUrlEncoded is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.FormUrlEncoded`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.FormUrlEncoded/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.FormUrlEncoded/blob/master/docs/RELEASE-NOTES.md).

## Usage

`FormUrlEncodedSerializer` is the primary class in the library and allows the serialization and deserialization of form URL encoded content.

Serialization can be performed on any object. However, only the public instance properties of the object will have their values serialized.

It should be noted that when serialization of a object's property occurs the `ToString` method will be called on the property.
The form URL encoded format is very flat so no serialization of inner types (e.g. a property's type's properties) or collection type's elements will occur.

The serializer supports:

- All built in .NET types:
  - Reference types (object, string, etc.)
  - Value types (integral and floating point numeric types, enum, bool, char, struct)
- Serialization specific settings via the `SerializeOptions` type parameter.
- Deserialization specific settings via the `DeserializeOptions` type parameter.
- Property attributes:
  - `FormUrlEncodedPropertyNameAttribute` which overrides the property name to use during serialization/deserialization.
  - `FormUrlEncodedIgnoreAttribute` which prevents a property from being used during serialization/deserialization.
  - `FormUrlEncodedValueConverterAttribute` which can be extended to provide a custom type conversion during serialization/deserialization (e.g., to a bespoke string enumeration).

### Code Examples

The follow demonstrates simple serialization and deserialization of a type including the use of the `FormUrlEncodedPropertyName` and `FormUrlEncodedIgnore` attributes.

```csharp
// Entitiy class (class you want to serialize/deserialize)

public class Employee
{
    public string Name { get; set; }

    public int Age { get; set; }

    [FormUrlEncodedPropertyName("emailAddress")]
    public string Email { get; set; }

    [FormUrlEncodedIgnore]
    public int PayGrade { get; set; }

    [FormUrlColorValueConverter] // inherited from FormUrlEncodedValueConverterAttribute
    public int OfficeWallColor { get; set; }
}
```

```csharp
// Serialize an object to a form URL encoded string

var employee = new Employee
{
    Name = "John Smith",
    Age = 50,
    Email = "john@somewhere.com",
    PayGrade = 5,
    OfficeWallColor = System.Drawing.Color.LightSteelBlue
};

string data = FormUrlEncodedSerializer.Serialize(employee);

// data == "Name=John+Smith&Age=50&emailAddress=john%40somewhere.com&OfficeWallColor=LightSteelBlue"
```

```csharp
// Deserialize a form URL encoded string to an object

string data = "Name=John+Smith&Age=50&emailAddress=john%40somewhere.com&PayGrade=5&OfficeWallColor=LightSteelBlue";

Employee employee = FormUrlEncodedSerializer.Deserialize<Employee>(data);

// employee.Name == "John Smith"
// employee.Age == 50
// employee.Email == "john@somewhere.com"
// employee.PayGrade == 0
// employee.OfficeWallColor == System.Drawing.Color.LightSteelBlue
```

---

### Serialize Options

Serializing options are supported via the `SerializeOptions` type parameter. 

Options include:

- `Encode` - Indicates if all public property names and values should be URI encoded when serializing. True by default.
- `EncodeSpaceAsPlus` - Indicates if spaces should be encoded as a plus when serializing. True by default. Property Encode must be true for this property to have any affect.
- `IgnoreIfDefault` - Indicates if a property should be ignored if it is set to it's default value. False by default.
- `IgnoreIfNull` - Indicates if a property should be ignored if it is set to null. True by default. If property IgnoreIfDefault is set to true this property is ignored.
- `EnumHandling` - Indicates how enums should be handled during serialization. Default handling is by number.

### Deserialize Options

Deserializing options are supported via the `DeserializeOptions` type parameter.

Options include:

- `Decode` - Indicates if all pair names and values should be URI decoded when deserializing. True by default.
- `DecodePlusAsSpace` -  Indicates if the plus sign should be decoded as a space when deserializing. True by default. Property Decode must be true for this property to have any affect.
- `EnumHandling` - Indicates how enums should be handled during deserialization. Default handling is by number.
