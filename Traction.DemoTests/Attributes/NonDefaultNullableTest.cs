using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class NonDefaultNullableTest {

        private NonDefaultConsumer GetConsumer() => new NonDefaultConsumer();

        [Test]
        public void NonDefault_PreconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(123));
        }

        [Test]
        public void NonDefault_PreconditionNullableMethod_DoesNotThrowIfNull() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonDefault_PreconditionNullableMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(123));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_DoesNotThrowIfNull() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(null));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(0));
        }
    }
}
