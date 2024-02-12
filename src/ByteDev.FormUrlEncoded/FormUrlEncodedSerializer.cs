using System;
using System.Collections;
using System.Collections.Generic;
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

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.HasIgnoreAttribute())
                    continue;

                object propertyValue = propertyInfo.GetValue(obj);

                if (propertyValue == null)
                {
                    if (options.IgnoreIfDefault || options.IgnoreIfNull)
                        continue;

                    sb.AppendKeyValue(propertyInfo.GetAttributeOrPropertyName(), string.Empty, options);
                }
                else
                {
                    if (options.IgnoreIfDefault)
                    {
                        var valuesDefault = propertyValue.GetType().GetDefault();

                        if (propertyValue.Equals(valuesDefault))
                            continue;
                    }

                    if (options.EnumHandling == EnumHandling.Number && propertyInfo.PropertyType.IsEnum)
                    {
                        propertyValue = GetEnumNumberFromName(propertyValue);
                    }

                    if (propertyInfo.HasValueConverterAttribute())
                    {
                        var valueConverter = Attribute.GetCustomAttributes(propertyInfo, typeof(FormUrlEncodedValueConverterAttribute)).FirstOrDefault() as FormUrlEncodedValueConverterAttribute;
                        sb.AppendKeySequenceValue(propertyInfo.GetAttributeOrPropertyName(), valueConverter.ToString(propertyValue), options);
                        continue;
                    }

                    if (propertyInfo.IsTypeList())
                    {
                        var sequence = propertyValue as IEnumerable;

                        sb.AppendKeySequenceValue(propertyInfo.GetAttributeOrPropertyName(), sequence.ToCsv(), options);
                    }
                    else
                    {
                        sb.AppendKeyValue(propertyInfo.GetAttributeOrPropertyName(), propertyValue.ToString(), options);
                    }
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
        /// <param name="options">Deserialize options.</param>
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

            List<PropertyInfo> propertiesWithAttr = typeof(T).GetPropertiesWithAttribute<FormUrlEncodedPropertyNameAttribute>().ToList();

            foreach (string strPair in strPairs)
            {
                var pair = new FormUrlEndcodedPair(strPair, options, propertiesWithAttr);

                if (!pair.HasValue)
                    continue;

                var propertyInfo = typeof(T).GetProperty(pair.Name);

                if (propertyInfo == null || propertyInfo.HasIgnoreAttribute())
                    continue;

                if (propertyInfo.HasValueConverterAttribute())
                {                    
                    var valueConverter = Attribute.GetCustomAttributes(propertyInfo, typeof(FormUrlEncodedValueConverterAttribute)).FirstOrDefault() as FormUrlEncodedValueConverterAttribute;
                    obj.SetPropertyValue(pair.Name, valueConverter.FromString(pair.Value));
                    continue;
                } 

                if (propertyInfo.IsTypeList())
                    obj.SetPropertyValue(pair.Name, pair.Value.ToList(','));
                else
                    obj.SetPropertyValue(pair.Name, pair.Value);
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