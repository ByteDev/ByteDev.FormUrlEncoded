namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Represents options when serializing.
    /// </summary>
    public class SerializeOptions
    {
        /// <summary>
        /// Indicates if all public property names and values should be URI encoded when
        /// serializing. True by default.
        /// </summary>
        public bool Encode { get; set; } = true;

        /// <summary>
        /// Indicates if spaces should be encoded as a plus when serializing. True by default.
        /// Property Encode must be true for this property to have any affect.
        /// </summary>
        public bool EncodeSpaceAsPlus { get; set; } = true;

        /// <summary>
        /// Indicates if a property should be ignored if it is set to null.
        /// True by default. If property IgnoreIfDefault is set to true this property is ignored.
        /// </summary>
        public bool IgnoreIfNull { get; set; } = true;

        /// <summary>
        /// Indicates if a property should be ignored if it is set to it's default value.
        /// False by default. If set to true property IgnoreIfNull's value is ignored.
        /// </summary>
        public bool IgnoreIfDefault { get; set; } = false;

        /// <summary>
        /// Indicates how enums should be handled during serialization.
        /// Default handling is by number.
        /// </summary>
        public EnumHandling EnumHandling { get; set; } = EnumHandling.Number;
    }
}