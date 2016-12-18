using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class NonDefaultNullableTest {

        private NonDefaultConsumer GetDemo() => new NonDefaultConsumer();

        [Test]
        public void NonDefault_PreconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(123));
        }

        [Test]
        public void NonDefault_PreconditionNullableMethod_DoesNotThrowIfNull() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonDefault_PreconditionNullableMethod_ThrowsIfDefault() {
            var demo = GetDemo();
            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(123));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_DoesNotThrowIfNull() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(null));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_ThrowsIfDefault() {
            var demo = GetDemo();
            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(0));
        }
    }
}
