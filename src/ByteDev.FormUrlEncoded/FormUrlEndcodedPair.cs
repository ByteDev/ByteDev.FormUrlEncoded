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

            // See if the (url)name is in the list of properties with PropertyName attribute list
            PropertyInfo attrProperty = propertiesWithAttr.GetByAttributeName(pairArray[0]);

            // ...and then if so, pop it out there.
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