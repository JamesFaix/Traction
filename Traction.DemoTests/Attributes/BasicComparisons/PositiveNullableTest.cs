using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for Positive with nullable types.
    /// </summary>
    [TestFixture]
    public class PositiveNullableTest {

        private PositiveConsumer GetDemo() => new PositiveConsumer();

        #region PreconditionMethod
        [Test]
        public void Positive_NullablePreconditionMethod_DoesNotThrowIfPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(1));
        }

        [Test]
        public void Positive_NullablePreconditionMethod_DoesNotThrowIfNull() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(null));
        }

        [Test]
        public void Positive_NullablePreconditionMethod_ThrowsIf0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(0));
        }

        [Test]
        public void Positive_NullablePreconditionMethod_ThrowsIfNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Positive_NullablePostconditionMethod_DoesNotThrowIfPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(1));
        }

        [Test]
        public void Positive_NullablePostconditionMethod_DoesNotThrowIfNull() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(null));
        }

        [Test]
        public void Positive_NullablePostconditionMethod_ThrowsIf0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(0));
        }

        [Test]
        public void Positive_NullablePostconditionMethod_ThrowsIfNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(-1));
        }
        #endregion

    }
}
