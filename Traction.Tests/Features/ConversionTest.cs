using System;
using System.Collections;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    /// <summary>
    /// Tests for contracts being applied to conversion operators.
    /// The implementation for conversions is 99% the same as methods.
    /// Correct conversion operator behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods, and these tests pass.
    /// </summary>
    [TestFixture]
    public class ConversionTest {

        private ConversionConsumer GetConsumer() => new ConversionConsumer();

        #region NormalConversion (int)
        [Test]
        public void Conversion_Normal_DoesNotThrow() {
            ConversionConsumer consumer = null;

            Assert.DoesNotThrow(() => {
                var x = (int)consumer;
            });
        }
        #endregion

        #region Precondition conversion (long)
        [Test]
        public void Conversion_Precondition_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = (long)consumer;
            });
        }

        [Test]
        public void Conversion_Precondition_ThrowsIfContractBroken() {
            ConversionConsumer consumer = null;

            Assert.Throws<ArgumentNullException>(() => {
                var x = (long)consumer;
            });
        }
        #endregion

        #region Postcondition conversion (string)
        // returns null if arg is null

        [Test]
        public void Conversion_Postcondition_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = (string)consumer;
            });
        }

        [Test]
        public void Conversion_Postcondition_ThrowsIfContractBroken() {
            ConversionConsumer consumer = null;

            Assert.Throws<PostconditionException>(() => {
                var x = (string)consumer;
            });
        }
        #endregion

        #region Pre and Postcondition conversion (ArrayList)
        //returns null if argument's _returnValue1 field is null

        [Test]
        public void Conversion_PreAndPostcondition_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            consumer.testField = "test";

            Assert.DoesNotThrow(() => {
                var x = (ArrayList)consumer;
            });
        }

        [Test]
        public void Conversion_PreAndPostcondition_ThrowsIfPreconditionBroken() {
            ConversionConsumer consumer = null;

            Assert.Throws<ArgumentNullException>(() => {
                var x = (ArrayList)consumer;
            });
        }

        [Test]
        public void Conversion_PreAndPostcondition_ThrowsIfPostconditionBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() => {
                var x = (ArrayList)consumer;
            });
        }
        #endregion
    }
}
