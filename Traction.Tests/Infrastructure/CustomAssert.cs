using System;
using System.Reflection;
using NUnit.Framework;

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
                    Assert.Fail($"Expected exception of type {exceptionType}, but none was thrown.");
                }
                catch (AssertionException) {
                    throw;
                }
                catch (Exception e) {
                    Assert.AreEqual(exceptionType, e.GetType());
                }
            }
        }

        /// <summary>
        /// If <paramref name="exceptionType"/> is non-null, asserts that invoking the given method with the given arguments
        /// throws the given exception type; otherwise asserts that the given invocation does not throw.
        /// </summary>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <param name="method">The method to invoke.</param>
        /// <param name="source">The instance for the method to be called on.</param>
        /// <param name="arguments">The arguments to the method.</param>
        public static void Throws(Type exceptionType, MethodInfo method, object source, object[] arguments = null) {
            if (method == null) throw new ArgumentNullException(nameof(method));
         
            arguments = arguments ?? new object[0];

            if (exceptionType == null) {
                Assert.DoesNotThrow(() => method.Invoke(source, arguments));
            }
            else {
                try {
                    method.Invoke(source, arguments);
                    Assert.Fail($"Expected exception of type {exceptionType}, but none was thrown.");
                }
                catch (TargetInvocationException e) {
                    Assert.AreEqual(exceptionType, e.InnerException.GetType());
                }
                catch (AssertionException) {
                    throw;
                }
                catch (Exception e) {
                    Assert.AreEqual(exceptionType, e.GetType());
                }
            }
        }
    }
}
