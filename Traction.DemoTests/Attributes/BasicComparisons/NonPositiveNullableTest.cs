using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for NonPositive with nullable types.
    /// </summary>
    [TestFixture]
    public class NonPositiveNullableTest {

        private NonPositiveConsumer GetDemo() => new NonPositiveConsumer();

        #region PreconditionMethod
        [Test]
        public void NonPositive_NullablePreconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(-1));
        }

        [Test]
        public void NonPositive_NullablePreconditionMethod_DoesNotThrowIf0() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(0));
        }

        [Test]
        public void NonPositive_NullablePreconditionMethod_DoesNotThrowIfNull() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionNullableMethod(null));
        }

        [Test]
        public void NonPositive_NullablePreconditionMethod_ThrowsIfPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionNullableMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonPositive_NullablePostconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(-1));
        }

        [Test]
        public void NonPositive_NullablePostconditionMethod_DoesNotThrowIf0() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(0));
        }

        [Test]
        public void NonPositive_NullablePostconditionMethod_DoesNotThrowIfNull() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionNullableMethod(null));
        }

        [Test]
        public void NonPositive_NullablePostconditionMethod_ThrowsIfPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionNullableMethod(1));
        }
        #endregion

    }
}
