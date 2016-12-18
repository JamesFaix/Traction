using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for NonNegative with nullable types.
    /// </summary>
    [TestFixture]
    public class NonNegativeNullableTest {

        private NonNegativeConsumer GetDemo() => new NonNegativeConsumer();

        #region PreconditionMethod
        [Test]
        public void NonNegative_NullablePreconditionMethod_DoesNotThrowIfPositive() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(1));
        }

        [Test]
        public void NonNegative_NullablePreconditionMethod_DoesNotThrowIf0() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonNegative_NullablePreconditionMethod_DoesNotThrowIfNull() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonNegative_NullablePreconditionMethod_ThrowsIfNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNegative_NullablePostconditionMethod_DoesNotThrowIfPositive() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(1));
        }

        [Test]
        public void NonNegative_NullablePostconditionMethod_DoesNotThrowIf0() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(0));
        }

        [Test]
        public void NonNegative_NullablePostconditionMethod_DoesNotThrowIfNull() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(null));
        }

        [Test]
        public void NonNegative_NullablePostconditionMethod_ThrowsIfNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(-1));
        }
        #endregion

    }
}
