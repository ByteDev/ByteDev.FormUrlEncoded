namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects
{
    internal class TestDummyCustomObjectProperty
    {
        public MyClass MyClass { get; set; }
    }

    internal class MyClass
    {
        public override string ToString()
        {
            return "ThisIsMyClass";
        }
    }
}