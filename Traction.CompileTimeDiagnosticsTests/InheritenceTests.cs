using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class InheritenceTests {

        [Test, TestCaseSource(nameof(AbstractMemberCases))]
        public void AbstractMemberTest(CSharpCompilation compilation) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            Assert.IsFalse(diagnostics.Any());
        }

        private static IEnumerable<TestCaseData> AbstractMemberCases {
            get {
                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"abstract class TestClass {
                            [return: NonNull] abstract string TestMethod();
                        }"))
                    .SetName($"Inheritance_AbstractMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"abstract class TestClass {
                            [NonNull] abstract string TestProperty { get; }
                        }"))
                    .SetName($"Inheritance_AbstractPropertiesCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest {
                            [return: NonNull] string TestMethod();
                        }"))
                    .SetName($"Inheritance_InterfaceMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest {
                            [NonNull] string TestProperty { get; }
                        }"))
                    .SetName($"Inheritance_InterfacePropertiesCompileWithoutError");
            }
        }

        [Test, TestCaseSource(nameof(InheritedMemberCases))]
        public void InheritedMemberTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsFalse(diagnostics.Any());
            }
            else {
                Assert.IsTrue(diagnostics.Any(d => d.Id == "TR0004"));
            }
        }

        private static IEnumerable<TestCaseData> InheritedMemberCases {
            get {
                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            void TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            public void TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"Inheritance_ImplicitInterfaceMethodImplementationsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            void TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            public void ITest.TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"Inheritance_ExplicitInterfaceMethodImplementationsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public abstract void TestMethod([Positive] int x);
                        }
                        class DerivedClass : BaseClass {
                            public override void TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"Inheritance_OverrideMethodsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            int TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            [return: NonDefault] public int TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"Inheritance_ImplicitInterfaceMethodImplementationsCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            int TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            [return: NonDefault] public int ITest.TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"Inheritance_ExplicitInterfaceMethodImplementationsCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public abstract int TestMethod([Positive] int x);
                        }
                        class DerivedClass : BaseClass {
                            [return: NonDefault] public override int TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"Inheritance_OverrideMethodCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            [Positive] int TestProperty { get; set; }
                        }
                        class TestClass : ITest {
                            [NonDefault] public int TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"Inheritance_ImplicitInterfacePropertyImplementationsCannotHaveContracts");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            [Positive] int TestProperty { get; set; }
                        }
                        class TestClass : ITest {
                            [NonDefault] public int ITest.TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"Inheritance_ExplicitInterfacePropertyImplementationsCannotHaveContracts");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            [Positive] public abstract int TestProperty { get; set; } 
                        }
                        class DerivedClass : BaseClass {
                            [NonDefault] public override int TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"Inheritance_OverridePropertiesCannotHaveContracts");

            }
        }
    }
}
