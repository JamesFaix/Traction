using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for contracts being applied to expression-bodied members.
    /// Correct expression-bodied member behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal properties and methods, and these tests pass.
    /// </summary>
    [TestFixture]
    public class ExpressionBodiedMemberTest {

        private ExpressionBodiedMemberConsumer GetConsumer() => new ExpressionBodiedMemberConsumer();

        #region NormalProperty
        [Test]
        public void ExpressionBodiedMember_NormalProperty_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalProperty;
            });
        }
        #endregion

        #region ContractProperty
        [Test]
        public void ExpressionBodiedMember_ContractProperty_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractProperty;
            });
        }
        #endregion

        #region NormalMethod
        [Test]
        public void ExpressionBodiedMember_NormalMethod_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }

        #endregion

        #region PostconditionMethod
        [Test]
        public void ExpressionBodiedMember_PostconditionMethod_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod());
        }
        #endregion
    }
}
