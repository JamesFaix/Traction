using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    /// <summary>
    /// Tests for Negative with nullable types.
    /// </summary>
    [TestFixture]
    public class NegativeNullableTest {

        private NegativeConsumer GetConsumer() => new NegativeConsumer();

        #region PreconditionMethod
        [Test]
        public void Negative_NullablePreconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(-1));
        }

        [Test]
        public void Negative_NullablePreconditionMethod_ThrowsIf0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void Negative_NullablePreconditionMethod_ThrowsIfPositive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Negative_NullablePostconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(-1));
        }

        [Test]
        public void Negative_NullablePostconditionMethod_ThrowsIf0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(0));
        }

        [Test]
        public void Negative_NullablePostconditionMethod_ThrowsIfPosiitive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(1));
        }
        #endregion

    }
}
