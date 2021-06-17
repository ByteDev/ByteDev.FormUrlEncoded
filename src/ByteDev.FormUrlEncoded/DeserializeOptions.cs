namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Represents options when deserializing.
    /// </summary>
    public class DeserializeOptions
    {
        /// <summary>
        /// Indicates if all pair names and values should be URI decoded when deserializing.
        /// True by default.
        /// </summary>
        public bool Decode { get; set; } = true;

        /// <summary>
        /// Indicates if the plus sign should be decoded as a space when deserializing.
        /// True by default. Property Decode must be true for this property to have any affect.
        /// </summary>
        public bool DecodePlusAsSpace { get; set; } = true;

        /// <summary>
        /// Indicates how enums should be handled during deserialization.
        /// Default handling is by number.
        /// </summary>
        public EnumHandling EnumHandling { get; set; } = EnumHandling.Number;
    }
}