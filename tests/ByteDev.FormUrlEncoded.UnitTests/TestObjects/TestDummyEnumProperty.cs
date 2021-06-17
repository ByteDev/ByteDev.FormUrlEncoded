namespace ByteDev.FormUrlEncoded.UnitTests.TestObjects
{
    internal class TestDummyEnumProperty
    {
        public TrafficLight TrafficLight { get; set; }
    }

    internal enum TrafficLight
    {
        Red = 1,
        Yellow = 2,
        Gree = 3
    }
}