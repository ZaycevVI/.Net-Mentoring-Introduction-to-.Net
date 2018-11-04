namespace DI.Tests.TestHelper
{
    // classes are needed to test behavior of IoC in different cases

    public interface ITestController1
    {
        ITest1 Test1 { get; set; }
        ITest2 Test2 { get; set; }
        int Count { get; set; }
    }

    public class TestController1 : ITestController1
    {
        public TestController1(ITest1 test)
        {
            Count = 1;
            Test1 = test;
        }

        // correct ctor to choose
        public TestController1(ITest1 test1, ITest2 test2)
        {
            Count = 2;
            Test1 = test1;
            Test2 = test2;
        }

        public ITest1 Test1 { get; set; }

        public ITest2 Test2 { get; set; }

        public int Count { get; set; }
    }

    public interface ITestController2 { }

    public class TestController2 : ITestController2
    {
        // eternal recursion, cyclical reference
        public TestController2(ITestController2 controller) { }
    }
}