using System;

namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// The FormUrlEncodedValueConverterAttribute class provides a base class all ValueConverters must derive from and implement.
    /// </summary>
    /// <remarks>
    /// FormUrlEncodedValueConverters are used to implement custom two-way conversions between your POCO property data type, 
    /// and a string that can be encoded in a FormUrlEncoded string.
    /// They are ideal for implementing a custom conversion without requiring extra properties with custom mappings.
    /// <para>To provide a custom FormUrlEncodedValueConverter for a property, inherit from this class and supply definitions for both conversion methods
    /// for your data type. Decorate the appropriate properties that require your FormUrlEncodedValueConverter with your derived attribute class.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class FormUrlEncodedValueConverterAttribute : FormUrlEncodedAttribute
    {
        /// <summary>
        /// Converts the given <paramref name="value"/> from a decoded string to your POCO's property type.
        /// </summary>
        /// <param name="value">The string value decoded from a FormUrlEncoded string to be converted to a custom type.</param>
        /// <returns>The converted property value.</returns>
        public abstract object ConvertFromString(string value);

        /// <summary>
        /// Converts the given <paramref name="value"/> from your POCO's property type to a plain string to be form URL encoded.
        /// </summary>
        /// <param name="value">The POCO value to be converted.</param>
        /// <returns>The converted property value.</returns>
        public abstract string ConvertToString(object value);
    }
}