using System.Reflection;
using CodeExecuter.Exceptions;
using CodeExecuter.Tests.TestObjects.Test1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeExecuter.Tests
{
    [TestClass]
    public class TypeBinderTests
    {
        private const string NAMESPACE = "CodeExecuter.Tests";
        private const string CLASS = "TestClass";

        [TestMethod]
        public void Should_CreateTypeInstanceWithoutNamespace()
        {
            var a = new TypeBinder(CLASS);
            Assert.IsTrue(a.Type == typeof(TestClass));
        }

        [TestMethod]
        public void Should_CreateTypeInstanceWithNamespace()
        {
            var a = new TypeBinder(NAMESPACE, CLASS);
            Assert.IsTrue(a.Type == typeof(TestClass));
        }

        [TestMethod]
        public void Should_ThrowErrorCouldnotResolveType()
        {
            try
            {
                var a = new TypeBinder("ClassWithSameName");
                Assert.Fail();
            }
            catch (CouldNotDetermineTypeException)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public void Should_LoadClassWithSameNameWhenNamespaceIsProvided()
        {
            var a = new TypeBinder("CodeExecuter.Tests.TestObjects.Test1", "ClassWithSameName");
            Assert.IsTrue(a.Type == typeof(ClassWithSameName));
        }

        [TestMethod]
        public void Should_ThrowErrorClassWasNotFound()
        {
            try
            {
                var a = new TypeBinder(NAMESPACE, "UnexistingClass");
                Assert.Fail();
            }
            catch (MissingAssemblyException)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void Should_ThrowErrorClassWasNotFoundWithoutNamespace()
        {
            try
            {
                var a = new TypeBinder("UnexistingClass");
                Assert.Fail();
            }
            catch (MissingAssemblyException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Should_ExecuteAdd()
        {
            var a = new TypeBinder(NAMESPACE, CLASS);
            var b = (int)a.Execute("Add", 1, 1);
            Assert.IsTrue(b == 2);
        }

        [TestMethod]
        public void Should_ThrowExceptioWhenNotEnoughParameters()
        {
            try
            {
                var a = new TypeBinder(NAMESPACE, CLASS);
                a.Execute("Add", 1);
            }
            catch (TargetParameterCountException)
            {
                Assert.IsTrue(true);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void Should_ExecuteWhenTooMuchParameters()
        {
            var binder = new TypeBinder(NAMESPACE, CLASS);
            var result = (int)binder.Execute("Add", 1, 1, 56);
            Assert.IsTrue(result == 2);
        }
    }
}
