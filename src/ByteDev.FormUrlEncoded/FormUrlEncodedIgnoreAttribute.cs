using System;

namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Represents an attribute that prevents a property from being serialized or deserialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FormUrlEncodedIgnoreAttribute : FormUrlEncodedAttribute
    {
    }
}