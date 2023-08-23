using System.Collections.Generic;
using System.Reflection;

namespace ByteDev.FormUrlEncoded
{
    internal class FormUrlEndcodedPair
    {
        public string Name { get; }

        public string Value { get; }

        public bool HasValue => Value != string.Empty;

        public FormUrlEndcodedPair(string pair, DeserializeOptions options, List<PropertyInfo> propertiesWithAttr)
        {
            var pairArray = pair.Split('=');

            PropertyInfo attrProperty = propertiesWithAttr.GetByAttributeName(pairArray[0]);

            if (attrProperty == null)
                Name = UrlEncoder.Decode(pairArray[0], options);
            else
                Name = attrProperty.Name;

            if (pairArray.Length == 2)
                Value = UrlEncoder.Decode(pairArray[1], options);
            else
                Value = string.Empty;
        }
    }
}