using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for NonPositive with nullable types.
    /// </summary>
    [TestFixture]
    public class NonPositiveNullableTest {

        private NonPositiveConsumer GetConsumer() => new NonPositiveConsumer();

        #region PreconditionMethod
        [Test]
        public void NonPositive_NullablePreconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(-1));
        }

        [Test]
        public void NonPositive_NullablePreconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonPositive_NullablePreconditionMethod_DoesNotThrowIfNull() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonPositive_NullablePreconditionMethod_ThrowsIfPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonPositive_NullablePostconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(-1));
        }

        [Test]
        public void NonPositive_NullablePostconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(0));
        }

        [Test]
        public void NonPositive_NullablePostconditionMethod_DoesNotThrowIfNull() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(null));
        }

        [Test]
        public void NonPositive_NullablePostconditionMethod_ThrowsIfPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(1));
        }
        #endregion

    }
}
