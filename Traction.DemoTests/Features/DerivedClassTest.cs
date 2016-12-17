using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    class DerivedClassTest {

        private DerivedClassDemo GetConsumer() => new DerivedClassDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<DerivedClassDemo> action, Type exceptionType) {
            var consumer = GetConsumer();
            CustomAssert.Throws(exceptionType, () => action(consumer));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<DerivedClassDemo> action;

                action = consumer => { consumer.PreconditionMethod("test"); };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PreconditionMethod_DoesNotThrowIfContractMet");

                action = consumer => { consumer.PreconditionMethod(null); };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"DerivedClass_PreconditionMethod_ThrowsIfContractBroken");

                action = consumer => { consumer.PostconditionMethod("test"); };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PostconditionMethod_DoesNotThrowIfContractMet");

                action = consumer => { consumer.PostconditionMethod(null); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"DerivedClass_PostconditionMethod_ThrowsIfContractBroken");

                action = consumer => { consumer.Property = "test"; };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PreconditionProperty_DoesNotThrowIfContractMet");

                action = consumer => { consumer.Property = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"DerivedClass_PreconditionProperty_ThrowsIfContractBroken");

                action = consumer => {
                    consumer._property = "test";
                    var x = consumer.Property;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"DerivedClass_PostconditionProperty_DoesNotThrowIfContractMet");

                action = consumer => { var x = consumer.Property; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"DerivedClass_PostconditionProperty_ThrowsIfContractBroken");
            }
        }
    }
}
