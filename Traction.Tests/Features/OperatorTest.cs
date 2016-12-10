using System;
using NUnit.Framework;
using Traction.TestConsumer;

namespace Traction.Tests {

    [TestFixture]
    public class OperatorTest {

        private OperatorConsumer GetConsumer() => new OperatorConsumer();

        #region Normal operator (+)
        [Test]
        public void Operator_Normal_DoesNotThrow() {
            var consumer1 = GetConsumer();
            var consumer2 = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 + consumer2;
            });
        }
        #endregion

        #region Precondition operator (-)
        [Test]
        public void Operator_Precondition_DoesNotThrowIfContractMet() {
            var consumer1 = GetConsumer();
            var consumer2 = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 - consumer2;
            });
        }

        [Test]
        public void Operator_Precondition_ThrowsIfContractBroken() {
            OperatorConsumer consumer1 = null;
            var consumer2 = GetConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                var x = consumer1 - consumer2;
            });
        }
        #endregion

        #region Postcondition operator (*)
        // * returns null if either parameter is null

        [Test]
        public void Operator_Postcondition_DoesNotThrowIfContractMet() {
            var consumer1 = GetConsumer();
            var consumer2 = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 * consumer2;
            });
        }

        [Test]
        public void Operator_Postcondition_ThrowsIfContractBroken() {
            var consumer1 = GetConsumer();
            OperatorConsumer consumer2 = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer1 * consumer2;
            });
        }
        #endregion

        #region Pre and Postcondition operator (/)
        // / returns null if both arguments are equal

        [Test]
        public void Operator_PreAndPost_DoesNotThrowContractMet() {
            var consumer1 = GetConsumer();
            var consumer2 = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 / consumer2;
            });
        }

        [Test]
        public void Operator_PreAndPost_ThrowsIfPreconditionBroken() {
            OperatorConsumer consumer1 = null;
            var consumer2 = GetConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                var x = consumer1 / consumer2;
            });
        }

        [Test]
        public void Operator_PreAndPost_ThrowsIfPostconditionBroken() {
            var consumer1 = GetConsumer();

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer1 / consumer1;
            });
        }
        #endregion
    }
}
