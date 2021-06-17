namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects
{
    internal class TestDummyBuiltInAllTypes
    {
        #region Value types

        public bool Bool { get; set; }

        public char Char { get; set; }

        public byte Byte { get; set; }

        public short Short { get; set; }

        public int Int { get; set; }

        public long Long { get; set; }

        public sbyte SByte { get; set; }

        public ushort UShort { get; set; }

        public uint UInt { get; set; }

        public ulong ULong { get; set; }

        public float Float { get; set; }

        public double Double { get; set; }

        public decimal Decimal { get; set; }

        #endregion

        #region Ref types

        public object Obj { get; set; }

        public string String { get; set; }

        #endregion
    }
}