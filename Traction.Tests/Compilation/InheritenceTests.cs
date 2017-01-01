using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Traction.Contracts.Analysis.DiagnosticCodes;
using static Traction.Roslyn.Rewriting.DiagnosticCodes;

namespace Traction.Tests.Compilation {

    [TestFixture]
    public class InheritenceTests {

        private const string fixture = "Compilation_Inheritance_";

        [Test, TestCaseSource(nameof(AbstractMemberCases))]
        public void AbstractMemberTest(CSharpCompilation compilation) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            Assert.IsTrue(diagnostics.ContainsOnlyCode(RewriteConfirmed));
        }

        private static IEnumerable<TestCaseData> AbstractMemberCases {
            get {
                const string test = "AbstractMembers_";

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"abstract class TestClass {
                            [return: NonNull] abstract string TestMethod();
                        }"))
                    .SetName($"{fixture}{test}AbstractMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"abstract class TestClass {
                            [NonNull] abstract string TestProperty { get; }
                        }"))
                    .SetName($"{fixture}{test}AbstractPropertiesCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest {
                            [return: NonNull] string TestMethod();
                        }"))
                    .SetName($"{fixture}{test}InterfaceMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest {
                            [NonNull] string TestProperty { get; }
                        }"))
                    .SetName($"{fixture}{test}InterfacePropertiesCompileWithoutError");
            }
        }

        [Test, TestCaseSource(nameof(InheritedMemberCases))]
        public void InheritedMemberTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsTrue(diagnostics.ContainsOnlyCode(RewriteConfirmed));
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(NoPreconditionsOnInheritedMembers));
            }
        }

        private static IEnumerable<TestCaseData> InheritedMemberCases {
            get {
                const string test = "Single_";
                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            void TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            public void TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}ImplicitInterfaceMethodImplementationsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            void TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            public void ITest.TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}ExplicitInterfaceMethodImplementationsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public abstract void TestMethod([Positive] int x);
                        }
                        class DerivedClass : BaseClass {
                            public override void TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}OverrideMethodsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            int TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            [return: NonDefault] public int TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}ImplicitInterfaceMethodImplementationsCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            int TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            [return: NonDefault] public int ITest.TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}ExplicitInterfaceMethodImplementationsCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public abstract int TestMethod([Positive] int x);
                        }
                        class DerivedClass : BaseClass {
                            [return: NonDefault] public override int TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}OverrideMethodCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            [Positive] int TestProperty { get; set; }
                        }
                        class TestClass : ITest {
                            [NonDefault] public int TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}ImplicitInterfacePropertyImplementationsCannotHaveContracts");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            [Positive] int TestProperty { get; set; }
                        }
                        class TestClass : ITest {
                            [NonDefault] public int ITest.TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}ExplicitInterfacePropertyImplementationsCannotHaveContracts");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            [Positive] public abstract int TestProperty { get; set; } 
                        }
                        class DerivedClass : BaseClass {
                            [NonDefault] public override int TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}OverridePropertiesCannotHaveContracts");

            }
        }

        [Test, TestCaseSource(nameof(MultipleInterfaceInheritanceCases))]
        public void MultipleInterfaceInheritanceTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsTrue(diagnostics.ContainsOnlyCode(RewriteConfirmed));
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(MembersCannotInheritPreconditionsFromMultipleSources));
            }
        }

        public static IEnumerable<TestCaseData> MultipleInterfaceInheritanceCases {
            get {
                const string test = "Multiple";

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface IFirst { 
                            void TestMethod(string text);
                        }
                        interface ISecond { 
                            void TestMethod(string text);
                        }  
                        class Implementer : IFirst, ISecond {
                            public void TestMethod(string text) { }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}MethodCanImplementMultipleInterfacesWithoutPreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface IFirst { 
                            void TestMethod([NonNull] string text);
                        }
                        interface ISecond { 
                            void TestMethod(string text);
                        }  
                        class Implementer : IFirst, ISecond {
                            public void TestMethod(string text) { }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}MethodCanImplementMultipleInterfacesIfOneHasAPrecondition");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public virtual void TestMethod([NonNull] string text) { }
                        }
                        interface ITest { 
                            void TestMethod(string text);
                        }  
                        class DerivedClass : BaseClass, ITest {
                            public override void TestMethod(string text) { }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}MethodCanOverrideBaseMethodWithPreconditionAndImplementInterfaceWithoutPrecondition");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public virtual void TestMethod(string text) { }
                        }
                        interface ITest { 
                            void TestMethod([NonNull] string text);
                        }  
                        class DerivedClass : BaseClass, ITest {
                            public override void TestMethod(string text) { }
                        }"
                        ), true)
                    .SetName($"{fixture}{test}MethodCanOverrideBaseMethodWithoutPreconditionAndImplementInterfaceWithPrecondition");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface IFirst { 
                            void TestMethod([NonNull] string text);
                        }
                        interface ISecond { 
                            void TestMethod([NonEmpty] string text);
                        }  
                        class Implementer : IFirst, ISecond {
                            public void TestMethod(string text) { }
                        }"
                        ), false)
                    .SetName($"{fixture}{test}MethodCannotImplementMultipleInterfacesWithPreconditions");

                yield return new TestCaseData(
                   CompilationFactory.CompileClassFromText(
                       @"class BaseClass { 
                            public virtual void TestMethod([NonNull] string text) { }
                        }
                        interface ITest { 
                            void TestMethod([NonEmpty] string text);
                        }  
                        class DerivedClass : BaseClass, ITest {
                            public override void TestMethod(string text) { }
                        }"
                       ), false)
                   .SetName($"{fixture}{test}MethodCannotBothOverrideBaseMethodWithPreconditionAndImplementInterfaceWithPrecondition");
            }
        }
    }
}
