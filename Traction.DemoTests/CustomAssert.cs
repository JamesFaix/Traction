using System;
using NUnit.Framework;

namespace Traction.DemoTests {

    static class CustomAssert {

        /// <summary>
        /// If <paramref name="exceptionType"/> is non-null, asserts that the given action
        /// throws the given exception type; otherwise asserts that the given expression does not throw.
        /// </summary>
        /// <param name="exceptionType">Type of the exception. (Can be null)</param>
        /// <param name="action">The action.</param>
        public static void Throws(Type exceptionType, Action action) {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (exceptionType == null) {
                Assert.DoesNotThrow(() => action());
            }
            else {
                try {
                    action();
                    Assert.Fail($"Expected exception of type {exceptionType}.");
                }
                catch (Exception e) {
                    Assert.AreEqual(exceptionType, e.GetType());
                }
            }
        }
    }
}
