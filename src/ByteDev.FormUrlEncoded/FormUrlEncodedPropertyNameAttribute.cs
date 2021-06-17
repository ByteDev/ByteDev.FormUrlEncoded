using System;

namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Represents an attributes that specifies the property name that is present in the form URL encoded data
    /// when serializing and deserializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FormUrlEncodedPropertyNameAttribute : FormUrlEncodedAttribute
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.FormUrlEncoded.FormUrlEncodedPropertyNameAttribute" /> class
        /// with the specified property name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public FormUrlEncodedPropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}