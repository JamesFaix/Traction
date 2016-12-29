using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.Roslyn;

namespace Traction.Contracts {

    public class DefaultValueContractProvider : IContractProvider {

        static DefaultValueContractProvider() {
            Instance = new DefaultValueContractProvider();
        }

        public static DefaultValueContractProvider Instance { get; }

        public IEnumerable<Contract> Contracts { get; }

        private DefaultValueContractProvider() {
            Contracts = new[] {
               new Contract(
                    name: "NonNull",
                    attributeType: typeof(NonNullAttribute),
                    exceptionMessage: "Value cannot be null.",
                    invalidTypeDiagnosticMessage: $"{nameof(NonNullAttribute)} can only be applied to members with reference types.",
                    getCondition: (expr, t) => SyntaxFactory.ParseExpression($"!global::System.Object.Equals({expr}, null)"),
                    isValidType: t => t.Type.IsReferenceType),

                new Contract(
                     name: "NonDefault",
                     attributeType: typeof(NonDefaultAttribute),
                     exceptionMessage: "Value cannot be default(T).",
                     invalidTypeDiagnosticMessage: $"[{nameof(NonDefaultAttribute)} should never throw an invalid type diagnostic.]",
                     getCondition: (expr, t) => {
                        var symbol = t.Type;
                        var typeName = symbol.FullName();

                        if (symbol.IsNullable()) { //If type is nullable (T?), compare to default(T) instead of default(T?)
                            typeName = (symbol as INamedTypeSymbol)
                                 .TypeArguments[0]
                                 .FullName();
                        }
                        return SyntaxFactory.ParseExpression($"!global::System.Object.Equals({expr}, default({typeName}))");
                     },
                     isValidType: t => true),

                new Contract(
                     name: "NonEmpty",
                     attributeType: typeof(NonEmptyAttribute),
                     exceptionMessage: "Sequence cannot be null or empty.",
                     invalidTypeDiagnosticMessage: $"{nameof(NonEmptyAttribute)} can only be applied to members with types implementing {nameof(IEnumerable)}<T>.",
                     getCondition: (expr, t) => {
                         var text = "";
                         if (!t.Type.IsValueType) { //Check for null if reference type.
                                 text += $"!global::System.Object.Equals({expr}, null) && ";
                         }
                         text += $"global::System.Linq.Enumerable.Any({expr})";
                         return SyntaxFactory.ParseExpression(text);
                     },
                     isValidType: t => t.Type.IsEnumerable())
            };
        }
    }
}
