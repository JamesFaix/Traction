using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    [TestFixture]
    public class NonDefaultGenericTypeTest {

        private NonDefaultConsumer GetConsumer() => new NonDefaultConsumer();

        [Test]
        public void NonDefault_PreconditionGenericMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();
            Func<int> function = () => 1;

            Assert.DoesNotThrow(() =>
                consumer.PreconditionGenericMethod(function));
        }

        [Test]
        public void NonDefault_PreconditionGenericMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionGenericMethod(null));
        }

        [Test]
        public void NonDefault_PostconditionGenericMethod_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();
            Func<int> function = () => 1;

            Assert.DoesNotThrow(() =>
                consumer.PostconditionGenericMethod(function));
        }

        [Test]
        public void NonDefault_PostconditionGenericMethod_ThrowsIfDefault() {
            var consumer = GetConsumer();
            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionGenericMethod(null));
        }
    }
}
