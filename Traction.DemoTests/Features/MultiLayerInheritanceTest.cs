using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    class MulitLayerInheritanceTest {

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test<TDemo>(TDemo demo, Action<TDemo> action, Type exceptionType) {
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                yield return new TestCaseData(
                        new MultiLayerInheritanceDemo(),
                        (Action<MultiLayerInheritanceDemo>)(demo => { demo.PreconditionMethod(null); }),
                        typeof(PreconditionException))
                    .SetName($"MultiLayerClassInheritance_PreconditionMethod_ThrowsIfContractBroken");

                yield return new TestCaseData(
                        new MultiLayerInterfaceInheritanceDemo(),
                        (Action<MultiLayerInterfaceInheritanceDemo>)(demo => { demo.PreconditionMethod(null); }),
                        typeof(PreconditionException))
                    .SetName($"MultiLayerInterfaceInheritance_PreconditionMethod_ThrowsIfContractBroken");
            }
        }
    }
}
