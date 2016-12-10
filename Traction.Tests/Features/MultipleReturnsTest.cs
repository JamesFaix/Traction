using NUnit.Framework;
using Traction.TestConsumer;

namespace Traction.Tests {

    [TestFixture]
    public class MultipleReturnsTest {

        private MultipleReturnsConsumer GetConsumer() => new MultipleReturnsConsumer();
        
        #region ContractProperty
        [Test]
        public void MultipleReturns_ContractProperty_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            //This property internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = "test1";
            consumer._returnValue2 = "test2";

            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                var x = consumer.ContractProperty;
                var y = consumer.ContractProperty;
                Assert.AreNotEqual(x, y);
            });
        }

        [Test]
        public void MultipleReturns_ContractProperty_ThrowsIfContractBroken() {
            var consumer = GetConsumer();
            //This property internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = null;
            consumer._returnValue2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<ReturnValueException>(() => {
                    var x = consumer.ContractProperty;
                });
            }
        }

        #endregion

        #region PostconditionMethod
        [Test]
        public void MultipleReturns_PostconditionMethod_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            //This method internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = "test1";
            consumer._returnValue2 = "test2";

            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                consumer.PostconditionMethod();
                consumer.PostconditionMethod();
            });
        }

        [Test]
        public void MultipleReturns_PostconditionMethod_ThrowsIfContractBroken() {
            var consumer = GetConsumer();
            //This method internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = null;
            consumer._returnValue2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<ReturnValueException>(() => {
                    consumer.PostconditionMethod();
                });
            }
        }
        #endregion

        #region ContractMethodWithStatementLambda

        [Test]
        public void MultipleReturns_PostconditionMethodWithStatementLambda_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer.PostconditionMethodWithStatementLambda();
            });
        }

        #endregion

    }
}
