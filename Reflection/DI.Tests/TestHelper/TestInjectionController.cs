using DI.Container.DI;

namespace DI.Tests.TestHelper
{
    // classes are needed to test behavior of IoC in different cases
    public class TestController3
    {
        // obj with default ctor
        [Inject]
        public ITest1 Test1 { get; set; }

        // obj with default ctor
        [Inject]
        public Test2 Test2 { get; set; }
    }

    public class TestController4
    {
        // No constructor to use
        [Inject]
        public ITest3 Test { get; set; }
    }

    public class TestController5
    {
        // Two ctors have same amount of args, and all of them are registred
        [Inject]
        public ITest5 Test { get; set; }
    }

    public interface ITestController6 { }
    public class TestController6 : ITestController6
    {
        // Cyclical reference
        [Inject]
        public ITestController6 Test { get; set; }
    }

    public class TestController7
    {
        // Choose ctor with max registred params for this obj
        [Inject]
        public ITest6 Test { get; set; }
    }
}