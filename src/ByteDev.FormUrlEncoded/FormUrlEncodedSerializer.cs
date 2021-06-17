using System;
using System.Linq;
using System.Reflection;
using System.Text;
using ByteDev.Reflection;

namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Represents a serializer for form URL encoded (x-www-form-urlencoded) content.
    /// </summary>
    public static class FormUrlEncodedSerializer
    {
        /// <summary>
        /// Serialize an object to a form URL encoded string.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Form URL encoded string.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="obj" /> is null.</exception>
        public static string Serialize(object obj)
        {
            return Serialize(obj, new SerializeOptions());
        }

        /// <summary>
        /// Serialize an object to a form URL encoded string.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="options">Serialize options.</param>
        /// <returns>Form URL encoded string.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="obj" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="options" /> is null.</exception>
        public static string Serialize(object obj, SerializeOptions options)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var sb = new StringBuilder();

            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (property.HasIgnoreAttribute())
                    continue;

                object propertyValue = property.GetValue(obj);

                if (propertyValue == null)
                {
                    if (options.IgnoreIfDefault || options.IgnoreIfNull)
                        continue;

                    sb.AppendKeyValue(property.GetAttributeOrPropertyName(), string.Empty, options);
                }
                else
                {
                    if (options.IgnoreIfDefault)
                    {
                        var valuesDefault = propertyValue.GetType().GetDefault();

                        if (propertyValue.Equals(valuesDefault))
                            continue;
                    }

                    if (options.EnumHandling == EnumHandling.Number && property.PropertyType.IsEnum)
                    {
                        propertyValue = GetEnumNumberFromName(propertyValue);
                    }

                    sb.AppendKeyValue(property.GetAttributeOrPropertyName(), propertyValue.ToString(), options);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Deserialize a form URL encoded string to an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="formUrlEncodedData">Form URL encoded string to deserialize.</param>
        /// <returns>Object of type <typeparamref name="T" />.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="formUrlEncodedData" /> is null or empty.</exception>
        /// <exception cref="T:System.FormatException">Matching property's type cannot be set to the value.</exception>
        public static T Deserialize<T>(string formUrlEncodedData) where T : new()
        {
            return Deserialize<T>(formUrlEncodedData, new DeserializeOptions());
        }

        /// <summary>
        /// Deserialize a form URL encoded string to an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="formUrlEncodedData">Form URL encoded string to deserialize.</param>
        /// <param name="options">Deserialize options. If null defaults will be used.</param>
        /// <returns>Object of type <typeparamref name="T" />.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="formUrlEncodedData" /> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="options" /> is null.</exception>
        /// <exception cref="T:System.FormatException">Matching property's type cannot be set to the value.</exception>
        public static T Deserialize<T>(string formUrlEncodedData, DeserializeOptions options) where T : new()
        {
            if (string.IsNullOrEmpty(formUrlEncodedData))
                throw new ArgumentException("Form URL encoded data was null or empty.", nameof(formUrlEncodedData));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var strPairs = formUrlEncodedData.Split('&');

            var obj = new T();

            var propertiesWithAttr = typeof(T).GetPropertiesWithAttribute<FormUrlEncodedPropertyNameAttribute>().ToList();

            foreach (var strPair in strPairs)
            {
                var pair = new FormUrlEndcodedPair(strPair, options);

                if (!pair.IsValid)
                    continue;

                var attrProperty = propertiesWithAttr.SingleOrDefault(p => p.GetAttributeName() == pair.Name);
                
                if (attrProperty == null)
                {
                    var property = typeof(T).GetProperty(pair.Name);

                    if (property == null || property.HasIgnoreAttribute())
                        continue;

                    obj.SetPropertyValue(pair.Name, pair.Value);
                }
                else
                {
                    // Property has FormUrlEncodedPropertyNameAttribute
                    if (typeof(T).GetProperty(attrProperty.Name).HasIgnoreAttribute())
                        continue;

                    obj.SetPropertyValue(attrProperty.Name, pair.Value);
                }
            }

            return obj;
        }

        private static object GetEnumNumberFromName(object propertyValue)
        {
            // e.g. input name: "Yellow", return number: 2
            return Convert.ChangeType(propertyValue, ((Enum)propertyValue).GetTypeCode());
        }
    }
}