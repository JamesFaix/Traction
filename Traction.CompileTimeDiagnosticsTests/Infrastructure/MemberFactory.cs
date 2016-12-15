using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Traction.DiagnosticsTests {

    //Make sure there are spaces before the member name, parameter names, and parameter types.
    static class MemberFactory {

        public static MethodDeclarationSyntax MethodWithPrecondition(string typeName, string attributeName) {
            if (typeName == null) throw new ArgumentNullException(nameof(typeName));
            if (attributeName == null) throw new ArgumentNullException(nameof(attributeName));

            return GetMethod()
                    .AddParameterListParameters(
                        Parameter(Identifier(" param1"))
                            .WithType(ParseTypeName(" " + typeName))
                            .AddAttributeLists(AttributeList()
                                .AddAttributes(Attribute(ParseName(attributeName)))));
        }

        public static MethodDeclarationSyntax MethodWithPostcondition(string typeName, string attributeName) {
            if (typeName == null) throw new ArgumentNullException(nameof(typeName));
            if (attributeName == null) throw new ArgumentNullException(nameof(attributeName));

            return GetMethod(typeName)
                .AddAttributeLists(AttributeList()
                    .WithTarget(AttributeTargetSpecifier(ParseToken("return")))
                    .AddAttributes(Attribute(ParseName(attributeName))));
        }

        private static MethodDeclarationSyntax GetMethod(string returnTypeName = null) =>
            MethodDeclaration(ParseTypeName(returnTypeName ?? "void"), Identifier(" TEST_METHOD"))
                .WithBody(Block());
    }
}
