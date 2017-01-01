using System;
using NUnit.Framework;
using System.Reflection;

namespace Traction.Tests.RuntimeBehaviors {

    [TestFixture]
    public class NonNullTest {

        private const string fixture = "Runtime_NonNull_";

        [Test]
        public void NonNullWorks() {

            var assembly = TestHelper.GetAssembly(
                @"class TestClass {
                    public string TestMethod([NonNull] string text) {
                        return text;
                    }
                }");

            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(typeof(PreconditionException), 
                method, instance, new object[] { null });

            CustomAssert.Throws(null,
                method, instance, new object[] { "test" });             
        }
    }
}
