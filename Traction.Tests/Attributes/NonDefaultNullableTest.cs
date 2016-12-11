using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    [TestFixture]
    public class NonDefaultNullableTest {

        private NonDefaultConsumer GetConsumer() => new NonDefaultConsumer();

        [Test]
        public void NonDefault_PreconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonDefault_PreconditionNullableMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<ArgumentException>(() =>
                consumer.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(0));
        }

        [Test]
        public void NonDefault_PostconditionNullableMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(null));
        }
    }
}
