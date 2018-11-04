using System;
using DI.Container.Constants;
using DI.Container.DI;
using DI.Tests.TestHelper;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DI.Tests
{
    [TestClass]
    public class StandardKernelTests
    {
        private IKernel _kernel;

        [TestInitialize]
        public void Initialize()
        {
            _kernel = new StandardKernel();
        }

        [TestMethod]
        public void Bind_RegistrationAlreadyExists_Exception()
        {
            _kernel.Bind<ITest1, Test1>();

            Assert.ThrowsException<ArgumentException>(() => _kernel.Bind<ITest1, Test1>(),
                string.Format(Error.ParameterWasAlreadyRegistred, typeof(ITest1)));

            _kernel.Bind<ITest1, Test1>("key");

            Assert.ThrowsException<ArgumentException>(() => _kernel.Bind<ITest1, Test1>("key"),
                string.Format(Error.KeyWasAlreadyRegistred, "key"));
        }

        [TestMethod]
        public void Get_BindToSelf_ExpectedInstance()
        {
            Assert.IsInstanceOfType(_kernel.Get<Test1>(), typeof(Test1));
            // or
            _kernel.Bind<Test1, Test1>();
            Assert.IsInstanceOfType(_kernel.Get<Test1>(), typeof(Test1));
        }

        [TestMethod]
        public void Get_NoRegistredTypeForInterface_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITest1>(),
                string.Format(Error.NoRegisteredTypeForInterface, typeof(ITest1)));
        }

        [TestMethod]
        public void Get_KeyIsInvalid_Exception()
        {
            _kernel.Bind<ITest1, Test1>("1");
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITest1>("1234"),
                string.Format(Error.KeyInvalidOrDoesntExist, "1234"));
        }

        [TestMethod]
        public void Get_InvalidArgumentType_Exception()
        {
            _kernel.Bind<ITest1, Test1>("123");
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITest2>("123"),
                string.Format(Error.NoInheritanceBetweenTypes, typeof(ITest2), typeof(Test1)));
        }

        [TestMethod]
        public void Get_GetRegistredType_ExpectedInstance()
        {
            _kernel.Bind<ITest1, Test1>();
            Assert.IsInstanceOfType(_kernel.Get<ITest1>(), typeof(Test1));
            _kernel.Bind<ITest2, Test2>("123");
            Assert.IsInstanceOfType(_kernel.Get<ITest2>("123"), typeof(Test2));
        }

        [TestMethod]
        public void Get_CallCorrectCtorWithMaxRegistredArgs_ExpectedInstance()
        {
            _kernel.Bind<ITest1, Test1>();
            Assert.AreEqual(_kernel.Get<TestController1>().Count, 1); // ctor with 1 arg
            _kernel.Bind<ITest2, Test2>();
            Assert.AreEqual(_kernel.Get<TestController1>().Count, 2); // ctor with 2 args
            _kernel.Bind<ITestController1, TestController1>("123");
            Assert.AreEqual(_kernel.Get<ITestController1>("123").Count, 2); // ctor with 2 args
        }

        [TestMethod]
        public void Get_CreateCorrectInstancesOfRegistredCtorsArgs_ExpectedInstance()
        {
            _kernel.Bind<ITest1, Test1>();
            _kernel.Bind<ITest2, Test2>();
            Assert.IsInstanceOfType(_kernel.Get<TestController1>().Test1, typeof(Test1));
            Assert.IsInstanceOfType(_kernel.Get<TestController1>().Test2, typeof(Test2));
            _kernel.Bind<ITestController1, TestController1>("123");
            Assert.IsInstanceOfType(_kernel.Get<ITestController1>("123").Test1, typeof(Test1));
            Assert.IsInstanceOfType(_kernel.Get<TestController1>("123").Test2, typeof(Test2));
        }

        [TestMethod]
        public void Get_NoCtorWithRegistredParamsAndNoDefaultCtor_Exception()
        {
            _kernel.Bind<ITest3, Test3>();
            _kernel.Bind<ITest3, Test3>("123");
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITest3>(),
                string.Format(Error.NoRegisteredTypeForInterface, typeof(Test3)));
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITest3>("123"),
                string.Format(Error.NoRegisteredTypeForInterface, typeof(Test3)));
        }

        [TestMethod]
        public void Get_UseDefaultCtor_ExpectedInstance()
        {
            _kernel.Bind<ITest4, Test4>();
            _kernel.Bind<ITest4, Test4>("123");
            Assert.IsInstanceOfType(_kernel.Get<ITest4>(), typeof(Test4));
            Assert.IsInstanceOfType(_kernel.Get<ITest4>("123"), typeof(Test4));
        }

        [TestMethod]
        public void Get_SameParamAsSourceInstanceRecyclingReference_Exception()
        {
            _kernel.Bind<ITestController2, TestController2>();
            _kernel.Bind<ITestController2, TestController2>("123");
            _kernel.Bind<ITestController6, TestController6>();
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITestController2>(),
                Error.CyclingReferenceDetected);
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<ITestController2>("123"),
                Error.CyclingReferenceDetected);
            // for prop injection
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<TestController6>(),
                Error.CyclingReferenceDetected);
        }

        [TestMethod]
        public void Get_FindTwoCtorsWithMaxAmountOfRegistredParams_Exception()
        {
            _kernel.Bind<ITest1, Test1>();
            _kernel.Bind<ITest2, Test2>();
            _kernel.Bind<ITest5, Test5>();
            Assert.ThrowsException<TypeLoadException>(() => _kernel.Get<Test5>(),
                Error.MoreThanOneContructorsMatches);
            // for property injection
            Assert.ThrowsException<TypeLoadException>(() => _kernel.Get<TestController5>(),
                Error.MoreThanOneContructorsMatches);
        }

        [TestMethod]
        public void Get_PropertyInjectionByRegistration_ExpectedType()
        {
            _kernel.Bind<ITest1, Test1>();
            Assert.IsInstanceOfType(_kernel.Get<TestController3>().Test1, typeof(Test1));
        }

        [TestMethod]
        public void Get_PropertyInjectionToSelf_ExpectedType()
        {
            _kernel.Bind<ITest1, Test1>();
            // ITest2 -> Test2 wasn't registred but instance was created!
            Assert.IsInstanceOfType(_kernel.Get<TestController3>().Test2, typeof(Test2));
            Assert.IsInstanceOfType(_kernel.Get<TestController3>().Test1, typeof(Test1));
        }

        [TestMethod]
        public void Get_PropertyInjectionNoCtorForProperty_Exception()
        {
            _kernel.Bind<ITest3, Test3>();
            Assert.ThrowsException<ArgumentException>(() => _kernel.Get<TestController3>(),
                string.Format(Error.NoMatchingCtorsForType, typeof(Test3)));
        }

        [TestMethod]
        public void Get_ChooseCtorWithMaxRegistredParamsForPropInjection_ExpectedInstance()
        {
            _kernel.Bind<ITest6, Test6>();
            _kernel.Bind<ITest1, Test1>();
            var objPropInj = _kernel.Get<TestController7>();
            Assert.AreEqual(objPropInj.Test.Count, 2); // ctor with 2 registred params
        }

        [TestMethod]
        public void AddAssembly_WorksAsExpected_ExpectedListOfInstances()
        {
            _kernel.AddAssembly(typeof(IService1));

            var obj1 = _kernel.Get<IService1>();
            var obj2 = _kernel.Get<IService4>();

            Assert.IsInstanceOfType(obj1, typeof(Service1));
            Assert.IsInstanceOfType(obj2, typeof(Service4));
        }
    }
}
