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
            Assert.IsTrue(diagnostics.ContainsOnlyCode(DiagnosticCode.RewriteConfirmed));
        }

        private static IEnumerable<TestCaseData> AbstractMemberCases {
            get {
                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"abstract class TestClass {
                            [return: NonNull] abstract string TestMethod();
                        }"))
                    .SetName($"Inheritance_AbstractMembers_AbstractMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"abstract class TestClass {
                            [NonNull] abstract string TestProperty { get; }
                        }"))
                    .SetName($"Inheritance_AbstractMembers_AbstractPropertiesCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest {
                            [return: NonNull] string TestMethod();
                        }"))
                    .SetName($"Inheritance_AbstractMembers_InterfaceMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest {
                            [NonNull] string TestProperty { get; }
                        }"))
                    .SetName($"Inheritance_AbstractMembers_InterfacePropertiesCompileWithoutError");
            }
        }

        [Test, TestCaseSource(nameof(InheritedMemberCases))]
        public void InheritedMemberTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsTrue(diagnostics.ContainsOnlyCode(DiagnosticCode.RewriteConfirmed));
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(DiagnosticCode.NoPreconditionsOnInheritedMembers));
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
                    .SetName($"Inheritance_Single_ImplicitInterfaceMethodImplementationsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            void TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            public void ITest.TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"Inheritance_Single_ExplicitInterfaceMethodImplementationsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public abstract void TestMethod([Positive] int x);
                        }
                        class DerivedClass : BaseClass {
                            public override void TestMethod([NonDefault] int x) { }
                        }"
                        ), false)
                    .SetName($"Inheritance_Single_OverrideMethodsCannotHavePreconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            int TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            [return: NonDefault] public int TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"Inheritance_Single_ImplicitInterfaceMethodImplementationsCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            int TestMethod([Positive] int x);
                        }
                        class TestClass : ITest {
                            [return: NonDefault] public int ITest.TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"Inheritance_Single_ExplicitInterfaceMethodImplementationsCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            public abstract int TestMethod([Positive] int x);
                        }
                        class DerivedClass : BaseClass {
                            [return: NonDefault] public override int TestMethod(int x) { return 1; }
                        }"
                        ), true)
                    .SetName($"Inheritance_Single_OverrideMethodCanHavePostconditions");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            [Positive] int TestProperty { get; set; }
                        }
                        class TestClass : ITest {
                            [NonDefault] public int TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"Inheritance_Single_ImplicitInterfacePropertyImplementationsCannotHaveContracts");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"interface ITest { 
                            [Positive] int TestProperty { get; set; }
                        }
                        class TestClass : ITest {
                            [NonDefault] public int ITest.TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"Inheritance_Single_ExplicitInterfacePropertyImplementationsCannotHaveContracts");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        @"class BaseClass { 
                            [Positive] public abstract int TestProperty { get; set; } 
                        }
                        class DerivedClass : BaseClass {
                            [NonDefault] public override int TestProperty { get; set; }
                        }"
                        ), false)
                    .SetName($"Inheritance_Single_OverridePropertiesCannotHaveContracts");

            }
        }

        [Test, TestCaseSource(nameof(MultipleInterfaceInheritanceCases))]
        public void MultipleInterfaceInheritanceTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsTrue(diagnostics.ContainsOnlyCode(DiagnosticCode.RewriteConfirmed));
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(DiagnosticCode.MembersCannotInheritContractsFromMultipleSources));
            }
        }

        public static IEnumerable<TestCaseData> MultipleInterfaceInheritanceCases {
            get {
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
                    .SetName($"Inheritance_Multiple_MethodCanImplementMultipleInterfacesWithoutPreconditions");

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
                    .SetName($"Inheritance_Multiple_MethodCanImplementMultipleInterfacesIfOneHasAPrecondition");

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
                    .SetName($"Inheritance_Multiple_MethodCanOverrideBaseMethodWithPreconditionAndImplementInterfaceWithoutPrecondition");

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
                    .SetName($"Inheritance_Multiple_MethodCanOverrideBaseMethodWithoutPreconditionAndImplementInterfaceWithPrecondition");
                
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
                    .SetName($"Inheritance_Multiple_MethodCannotImplementMultipleInterfacesWithPreconditions");

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
                   .SetName($"Inheritance_Multiple_MethodCannotBothOverrideBaseMethodWithPreconditionAndImplementInterfaceWithPrecondition");
            }
        }
    }
}
