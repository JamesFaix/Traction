using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    class InterfaceTest {
        
        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(IDemo demo, Action<IDemo> action, Type exceptionType) {
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<IDemo> action;

                action = demo => { demo.PreconditionMethod("test"); };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, null)
                    .SetName($"ImplicitInterfaceImplementation_PreconditionMethod_DoesNotThrowIfContractMet");

                action = demo => { demo.PreconditionMethod(null); };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, typeof(PreconditionException))
                    .SetName($"ImplicitInterfaceImplementation_PreconditionMethod_ThrowsIfContractBroken");

                action = demo => { demo.PostconditionMethod("test"); };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, null)
                    .SetName($"ImplicitInterfaceImplementation_PostconditionMethod_DoesNotThrowIfContractMet");

                action = demo => { demo.PostconditionMethod(null); };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, typeof(PostconditionException))
                    .SetName($"ImplicitInterfaceImplementation_PostconditionMethod_ThrowsIfContractBroken");

                action = demo => { demo.Property = "test"; };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, null)
                    .SetName($"ImplicitInterfaceImplementation_PreconditionProperty_DoesNotThrowIfContractMet");

                action = demo => { demo.Property = null; };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, typeof(PreconditionException))
                    .SetName($"ImplicitInterfaceImplementation_PreconditionProperty_ThrowsIfContractBroken");

                action = demo => {
                    ((dynamic)(demo))._property = "test";
                    var x = demo.Property;
                };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, null)
                    .SetName($"ImplicitInterfaceImplementation_PostconditionProperty_DoesNotThrowIfContractMet");

                action = demo => { var x = demo.Property; };
                yield return new TestCaseData(new ImplicitInterfaceDemo(), action, typeof(PostconditionException))
                    .SetName($"ImplicitInterfaceImplementation_PostconditionProperty_ThrowsIfContractBroken");


                action = demo => { demo.PreconditionMethod("test"); };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, null)
                    .SetName($"ExplicitInterfaceImplementation_PreconditionMethod_DoesNotThrowIfContractMet");

                action = demo => { demo.PreconditionMethod(null); };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, typeof(PreconditionException))
                    .SetName($"ExplicitInterfaceImplementation_PreconditionMethod_ThrowsIfContractBroken");

                action = demo => { demo.PostconditionMethod("test"); };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, null)
                    .SetName($"ExplicitInterfaceImplementation_PostconditionMethod_DoesNotThrowIfContractMet");

                action = demo => { demo.PostconditionMethod(null); };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, typeof(PostconditionException))
                    .SetName($"ExplicitInterfaceImplementation_PostconditionMethod_ThrowsIfContractBroken");

                action = demo => { demo.Property = "test"; };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, null)
                    .SetName($"ExplicitInterfaceImplementation_PreconditionProperty_DoesNotThrowIfContractMet");

                action = demo => { demo.Property = null; };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, typeof(PreconditionException))
                    .SetName($"ExplicitInterfaceImplementation_PreconditionProperty_ThrowsIfContractBroken");

                action = demo => {
                    ((dynamic)(demo))._property = "test";
                    var x = demo.Property;
                };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, null)
                    .SetName($"ExplicitInterfaceImplementation_PostconditionProperty_DoesNotThrowIfContractMet");

                action = demo => { var x = demo.Property; };
                yield return new TestCaseData(new ExplicitInterfaceDemo(), action, typeof(PostconditionException))
                    .SetName($"ExplicitInterfaceImplementation_PostconditionProperty_ThrowsIfContractBroken");
            }
        }
    }
}
