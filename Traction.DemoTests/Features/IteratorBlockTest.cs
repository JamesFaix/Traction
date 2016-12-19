using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class IteratorBlockTest {

        private IteratorBlockDemo GetDemo() => new IteratorBlockDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<IteratorBlockDemo> action, Type exceptionType) {
            var demo = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<IteratorBlockDemo> action;

                action = demo => demo.NormalNonIteratorMethod(-1);
                yield return new TestCaseData(action, null)
                    .SetName($"IteratorBlock_NormalNonIteratorMethod_DoesNotThrow");

                action = demo => demo.NormalIteratorMethod(-1);
                yield return new TestCaseData(action, null)
                    .SetName($"IteratorBlock_NormalIteratorMethod_DoesNotThrow");

                action = demo => demo.PreconditionNonIteratorMethod(1);
                yield return new TestCaseData(action, null)
                    .SetName($"IteratorBlock_PreconditionNonIteratorMethod_DoesNotThrowIfContractMet");

                action = demo => demo.PreconditionNonIteratorMethod(-1);
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"IteratorBlock_PreconditionNonIteratorMethod_ThrowsIfContractBroken");

                action = demo => demo.PreconditionIteratorMethod(1);
                yield return new TestCaseData(action, null)
                    .SetName($"IteratorBlock_PreconditionIteratorMethod_DoesNotThrowIfContractMet");

                action = demo => demo.PreconditionIteratorMethod(-1);
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"IteratorBlock_PreconditionIteratorMethod_ThrowsIfContractBroken");
            }
        }
    }
}
