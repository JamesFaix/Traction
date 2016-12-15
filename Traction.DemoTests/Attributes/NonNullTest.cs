using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic NonNull property and method use cases.
    /// </summary>
    [TestFixture]
    public class NonNullTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void NonNull_NormalProperty_GetDoesNotThrow() {
            var consumer = new NonNullConsumer();
            consumer._normalProperty = null;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_NormalProperty_SetDoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = null;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void NonNull_ContractReadWriteProperty_GetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_GetThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = null;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = "test";
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = null;
            });
        }
        #endregion
                
        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonNull_NormalMethod_DoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonNull_PreconditionMethod_DoesNotThrowIfArgNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod("test"));
        }

        [Test]
        public void NonNull_PreconditionMethod_ThrowsIfArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNull_PostconditionMethod_DoesNotThrowIfResultNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod("test"));
        }

        [Test]
        public void NonNull_PostconditionMethod_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonNull_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNotNull() {
            var consumer = new NonNullConsumer();
            Func<string> textGenerator = () => "test";

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod(textGenerator));
        }

        [Test]
        public void NonNull_PreAndPostconditionMethod_ThrowsIfArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(null));
        }

        [Test]
        public void NonNull_PreAndPostconditionMethod_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionMethod(textGenerator));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonNull_MultiplePreconditionsMethod_DoesNotThrowIfNoArgsNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod("test", "testing"));
        }

        [Test]
        public void NonNull_MultiplePreconditionsMethod_ThrowsIfAnyArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod("test", null));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(null, "test"));
        }
        #endregion
        
        #endregion
    }
}
