using System;
using System.Collections.Generic;
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

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<ExpressionBodiedMemberConsumer> action, Type exceptionType) {
            var consumer = GetConsumer();
            CustomAssert.Throws(exceptionType, () => action(consumer));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<ExpressionBodiedMemberConsumer> action;

                action = consumer => { var x = consumer.NormalProperty; };
                yield return new TestCaseData(action, null)
                    .SetName($"ExpressionBodiedMember_NormalProperty_DoesNotThrow");

                action = consumer => { var x = consumer.ContractProperty; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"ExpressionBodiedMember_ContractProperty_ThrowsIfContractBroken");

                action = consumer => { consumer.NormalMethod(); };
                yield return new TestCaseData(action, null)
                    .SetName($"ExpressionBodiedMember_NormalMethod_DoesNotThrow");

                action = consumer => { consumer.PostconditionMethod(); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"ExpressionBodiedMember_PostconditionMethod_ThrowsIfContractBroken");
            }
        }
    }
}
