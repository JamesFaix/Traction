using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    /// <summary>
    /// Tests for Positive with nullable types.
    /// </summary>
    [TestFixture]
    public class PositiveNullableTest {

        private PositiveConsumer GetConsumer() => new PositiveConsumer();

        #region PreconditionMethod
        [Test]
        public void Positive_NullablePreconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionNullableMethod(1));
        }

        [Test]
        public void Positive_NullablePreconditionMethod_ThrowsIf0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(0));
        }

        [Test]
        public void Positive_NullablePreconditionMethod_ThrowsIfNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionNullableMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Positive_NullablePostconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionNullableMethod(1));
        }

        [Test]
        public void Positive_NullablePostconditionMethod_ThrowsIf0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(0));
        }

        [Test]
        public void Positive_NullablePostconditionMethod_ThrowsIfNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionNullableMethod(-1));
        }
        #endregion

    }
}
