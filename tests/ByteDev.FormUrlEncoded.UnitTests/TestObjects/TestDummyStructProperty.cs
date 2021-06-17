namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects
{
    public class TestDummyStructProperty
    {
        public TestStruct MyStruct { get; set; }
    }

    public struct TestStruct
    {
        public int Number { get; }

        public TestStruct(int number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return "ThisIsMyStruct";
        }
    }
}