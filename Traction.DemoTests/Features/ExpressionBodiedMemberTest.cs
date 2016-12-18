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

        private ExpressionBodyDemo GetDemo() => new ExpressionBodyDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<ExpressionBodyDemo> action, Type exceptionType) {
            var demo = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<ExpressionBodyDemo> action;

                action = demo => { var x = demo.NormalProperty; };
                yield return new TestCaseData(action, null)
                    .SetName($"ExpressionBodiedMember_NormalProperty_DoesNotThrow");

                action = demo => { var x = demo.ContractProperty; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"ExpressionBodiedMember_ContractProperty_ThrowsIfContractBroken");

                action = demo => { demo.NormalMethod(); };
                yield return new TestCaseData(action, null)
                    .SetName($"ExpressionBodiedMember_NormalMethod_DoesNotThrow");

                action = demo => { demo.PostconditionMethod(); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"ExpressionBodiedMember_PostconditionMethod_ThrowsIfContractBroken");
            }
        }
    }
}
