namespace ByteDev.FormUrlEncoded
{
    /// <summary>
    /// Defines how enums should be handled during serialization/deserialization.
    /// </summary>
    public enum EnumHandling
    {
        /// <summary>
        /// Handle the enum by it's number value.
        /// </summary>
        Number = 1,

        /// <summary>
        /// Handle the enum by it's name.
        /// </summary>
        Name = 2
    }
}