using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.Roslyn;

namespace Traction.Contracts {

    public class BasicComparisonContractProvider : IContractProvider {

        static BasicComparisonContractProvider() {
            Instance = new BasicComparisonContractProvider();
        }

        public static BasicComparisonContractProvider Instance { get; }

        public IEnumerable<Contract> Contracts { get; }

        private BasicComparisonContractProvider() {
            Contracts = new[] {
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
