using System;
using NUnit.Framework;
using System.Reflection;

namespace Traction.Tests {

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

        public static void Throws(Type exceptionType, MethodInfo method, object source, object[] arguments = null) {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (source == null) throw new ArgumentNullException(nameof(source));

            arguments = arguments ?? new object[0];

            if (exceptionType == null) {
                Assert.DoesNotThrow(() => method.Invoke(source, arguments));
            }
            else {
                try {
                    method.Invoke(source, arguments);
                    Assert.Fail($"Expected exception of type {exceptionType}.");
                }
                catch (TargetInvocationException e) {
                    Assert.AreEqual(exceptionType, e.InnerException.GetType());
                }
                catch {
                    Assert.Fail($"Expected exception of type {exceptionType}.");
                }
            }
        }
    }
}
