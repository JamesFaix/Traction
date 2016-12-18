using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class NonDefaultGenericTypeTest {

        private NonDefaultConsumer GetDemo() => new NonDefaultConsumer();

        [Test]
        public void NonDefault_PreconditionGenericMethod_DoesNotThrowIfNotDefault() {
            var demo = GetDemo();
            Func<int> function = () => 1;

            Assert.DoesNotThrow(() =>
                demo.PreconditionGenericMethod(function));
        }

        [Test]
        public void NonDefault_PreconditionGenericMethod_ThrowsIfDefault() {
            var demo = GetDemo();
            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionGenericMethod(null));
        }

        [Test]
        public void NonDefault_PostconditionGenericMethod_DoesNotThrowIfNotDefault() {
            var demo = GetDemo();
            Func<int> function = () => 1;

            Assert.DoesNotThrow(() =>
                demo.PostconditionGenericMethod(function));
        }

        [Test]
        public void NonDefault_PostconditionGenericMethod_ThrowsIfDefault() {
            var demo = GetDemo();
            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionGenericMethod(null));
        }
    }
}
