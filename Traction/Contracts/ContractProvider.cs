using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Contracts {

    public class ContractProvider : IContractProvider {

        static ContractProvider() {
            Instance = new ContractProvider();
        }

        public static ContractProvider Instance { get; }

        public IEnumerable<Contract> Contracts { get; }

        private ContractProvider() {
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
                     isValidType: t => t.Type.IsEnumerable()),

                 CreateBasicComparisonContract(
                    name: "Positive",
                    attributeType: typeof(PositiveAttribute),
                    operatorLiteral: ">",
                    operatorDescription: "greater than"),

                CreateBasicComparisonContract(
                    name: "Negative",
                    attributeType: typeof(NegativeAttribute),
                    operatorLiteral: "<",
                    operatorDescription: "less than"),

                CreateBasicComparisonContract(
                    name: "NonPositive",
                    attributeType: typeof(NonPositiveAttribute),
                    operatorLiteral: "<=",
                    operatorDescription: "less than or equal to"),

               CreateBasicComparisonContract(
                    name: "NonNegative",
                    attributeType: typeof(NonNegativeAttribute),
                    operatorLiteral: ">=",
                    operatorDescription: "greater than or equal to")
            };
        }

        private static Contract CreateBasicComparisonContract(string name, Type attributeType,
            string operatorLiteral, string operatorDescription) {
            return new Contract(
                name: name,
                attributeType: attributeType,
                exceptionMessage: $"Value must be {operatorDescription} default(T).",
                invalidTypeDiagnosticMessage: $"{attributeType} can only be applied to members with value types implementing {nameof(IComparable)}<T>, " +
                    $"or nullable types whose underlying type implements {nameof(IComparable)}<T>." +
                    "(Since the default value of all reference types is null, a comparison with default(T) would just be a null check.)",
                getCondition: (expr, t) => {
                    var symbol = t.Type;
                    string text;

                    if (symbol.IsNullable()) { //If type is nullable (T?), compare to default(T) instead of default(T?)
                        var typeName = (symbol as INamedTypeSymbol).TypeArguments[0].FullName();
                        text = $"!{expr}.HasValue || {expr}.Value.CompareTo(default({typeName})) {operatorLiteral} 0";
                    }
                    else {
                        var typeName = symbol.FullName();
                        text = $"{expr}.CompareTo(default({typeName})) {operatorLiteral} 0";
                    }

                    return SyntaxFactory.ParseExpression(text);
                },
                isValidType: t => {
                    var symbol = t.Type;

                    if (symbol.IsNullable()) {
                        return (symbol as INamedTypeSymbol).TypeArguments[0].IsComparable();
                    }
                    else {
                        return symbol.IsValueType && symbol.IsComparable();
                    }
                });
        }
    }
}
