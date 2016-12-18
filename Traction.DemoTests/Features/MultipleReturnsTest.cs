using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Test for postconditions being applied to members with multiple return statements or nested methods.
    /// Correct multiple-return-statement behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods/properties, and these tests pass.
    /// </summary>
    [TestFixture]
    public class MultipleReturnsTest {

        private MultipleReturnsDemo GetDemo() => new MultipleReturnsDemo();
        
        #region ContractProperty
        [Test]
        public void MultipleReturns_ContractProperty_DoesNotThrowIfContractMet() {
            var demo = GetDemo();
            //This property internally toggles between returning two fields from two return statements.
            demo._returnValue1 = "test1";
            demo._returnValue2 = "test2";

            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                var x = demo.ContractProperty;
                var y = demo.ContractProperty;
                Assert.AreNotEqual(x, y);
            });
        }

        [Test]
        public void MultipleReturns_ContractProperty_ThrowsIfContractBroken() {
            var demo = GetDemo();
            //This property internally toggles between returning two fields from two return statements.
            demo._returnValue1 = null;
            demo._returnValue2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<PostconditionException>(() => {
                    var x = demo.ContractProperty;
                });
            }
        }

        #endregion

        #region PostconditionMethod
        [Test]
        public void MultipleReturns_PostconditionMethod_DoesNotThrowIfContractMet() {
            var demo = GetDemo();
            //This method internally toggles between returning two fields from two return statements.
            demo._returnValue1 = "test1";
            demo._returnValue2 = "test2";

            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                demo.PostconditionMethod();
                demo.PostconditionMethod();
            });
        }

        [Test]
        public void MultipleReturns_PostconditionMethod_ThrowsIfContractBroken() {
            var demo = GetDemo();
            //This method internally toggles between returning two fields from two return statements.
            demo._returnValue1 = null;
            demo._returnValue2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<PostconditionException>(() => {
                    demo.PostconditionMethod();
                });
            }
        }
        #endregion

        #region ContractMethodWithStatementLambda

        [Test]
        public void MultipleReturns_PostconditionMethodWithStatementLambda_DoesNotThrow() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                var x = demo.PostconditionMethodWithStatementLambda();
            });
        }

        #endregion

    }
}
