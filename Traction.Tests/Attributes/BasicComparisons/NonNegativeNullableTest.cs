using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    /// <summary>
    /// Tests for NonNegative with nullable types.
    /// </summary>
    [TestFixture]
    public class NonNegativeNullableTest {

        private NonNegativeConsumer GetConsumer() => new NonNegativeConsumer();

        #region PreconditionMethod
        [Test]
        public void NonNegative_NullablePreconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(1));
        }

        [Test]
        public void NonNegative_NullablePreconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonNegative_NullablePreconditionMethod_DoesNotThrowIfNull() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonNegative_NullablePreconditionMethod_ThrowsIfNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNegative_NullablePostconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(1));
        }

        [Test]
        public void NonNegative_NullablePostconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(0));
        }

        [Test]
        public void NonNegative_NullablePostconditionMethod_DoesNotThrowIfNull() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(null));
        }

        [Test]
        public void NonNegative_NullablePostconditionMethod_ThrowsIfNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(-1));
        }
        #endregion

    }
}
