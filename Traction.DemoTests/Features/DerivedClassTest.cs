using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    class DerivedClassTest {

        private DerivedClassDemo GetDemo() => new DerivedClassDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<DerivedClassDemo> action, Type exceptionType) {
            var demo = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<DerivedClassDemo> action;

                action = demo => { demo.PreconditionMethod("test"); };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PreconditionMethod_DoesNotThrowIfContractMet");

                action = demo => { demo.PreconditionMethod(null); };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"DerivedClass_PreconditionMethod_ThrowsIfContractBroken");

                action = demo => { demo.PostconditionMethod("test"); };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PostconditionMethod_DoesNotThrowIfContractMet");

                action = demo => { demo.PostconditionMethod(null); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"DerivedClass_PostconditionMethod_ThrowsIfContractBroken");

                action = demo => { demo.Property = "test"; };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PreconditionProperty_DoesNotThrowIfContractMet");

                action = demo => { demo.Property = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"DerivedClass_PreconditionProperty_ThrowsIfContractBroken");

                action = demo => {
                    demo._property = "test";
                    var x = demo.Property;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PostconditionProperty_DoesNotThrowIfContractMet");

                action = demo => { var x = demo.Property; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"DerivedClass_PostconditionProperty_ThrowsIfContractBroken");
            }
        }
    }
}
