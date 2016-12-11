using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    [TestFixture]
    public class NonNullNullableTest {

        private NonNullConsumer GetConsumer() => new NonNullConsumer();

        [Test]
        public void NonNull_PreconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonNull_PreconditionNullableMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<ArgumentNullException>(() =>
                consumer.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonNull_PostconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(0));
        }

        [Test]
        public void NonNull_PostconditionNullableMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<ReturnValueException>(() =>
                consumer.PostconditionNullableMethod(null));
        }
    }
}
