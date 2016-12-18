using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for Negative with nullable types.
    /// </summary>
    [TestFixture]
    public class NegativeNullableTest {

        private NegativeConsumer GetDemo() => new NegativeConsumer();

        #region PreconditionMethod
        [Test]
        public void Negative_NullablePreconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(-1));
        }

        [Test]
        public void Negative_NullablePreconditionMethod_DoesNotThrowIfNull() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(null));
        }

        [Test]
        public void Negative_NullablePreconditionMethod_ThrowsIf0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(0));
        }

        [Test]
        public void Negative_NullablePreconditionMethod_ThrowsIfPositive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Negative_NullablePostconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(-1));
        }

        [Test]
        public void Negative_NullablePostconditionMethod_DoesNotThrowIfNull() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(null));
        }

        [Test]
        public void Negative_NullablePostconditionMethod_ThrowsIf0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(0));
        }

        [Test]
        public void Negative_NullablePostconditionMethod_ThrowsIfPosiitive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(1));
        }
        #endregion

    }
}
