using System;

namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Represents an attribute that handles value conversion between types
    /// when serializing and deserializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class FormUrlEncodedValueConverterAttribute : FormUrlEncodedAttribute
    {
        public abstract object FromString(string input);

        public abstract string ToString(object output);
    }
}