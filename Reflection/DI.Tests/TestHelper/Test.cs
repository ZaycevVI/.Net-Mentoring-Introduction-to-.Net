namespace DI.Tests.TestHelper
{
    // classes are needed to test behavior of IoC in different cases

    public interface ITest2 { }

    public interface ITest1 { }

    public class Test1 : ITest1 { }

    public class Test2 : ITest2 { }

    public interface ITest3 { }

    public class Test3 : ITest3
    {
        // Exception
        public Test3(string s) { }
    }

    public interface ITest4 { }

    public class Test4 : ITest4
    {
        // correct ctor to choose
        public Test4() { }

        public Test4(string s) { }
    }

    public interface ITest5 { }
    public class Test5 : ITest5
    {
        // don't know what ctor to choose
        public Test5(ITest1 test) { }
        // don't know what ctor to choose
        public Test5(ITest2 test) { }
    }

    public interface ITest6
    {
        int Count { get; set; }
    }
    public class Test6 : ITest6
    {
        public int Count { get; set; }

        // ctor to choose
        public Test6(ITest1 test1, ITest1 test2)
        {
            Count = 2;
        }

        public Test6(ITest1 test)
        {
            Count = 1;
        }
    }
}