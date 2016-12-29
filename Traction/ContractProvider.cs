using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.Roslyn.Semantics;

namespace Traction.Contracts {

    internal class ContractProvider : IContractProvider {

        #region Singleton

        static ContractProvider() {
            Instance = new ContractProvider();
        }

        public static ContractProvider Instance { get; }

        #endregion

        private ContractProvider() {
            this.contracts =
                new[] { NonNullContract, NonDefaultContract, NonEmptyContract }
                .Concat(BasicComparisonContracts)
                .ToDictionary(c => c.AttributeType, c => c);
        }

        private readonly Dictionary<Type, Contract> contracts;

        public IEnumerable<Contract> Contracts => contracts.Values;

        public Contract this[Type attributeType] => contracts[attributeType];

        #region Contract factory properties

        private static Contract NonDefaultContract {
            get {
                return new Contract(
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
                    isValidType: t => true);
            }
        }

        private static Contract NonNullContract {
            get {
                return new Contract(
                    name: "NonNull",
                    attributeType: typeof(NonNullAttribute),
                    exceptionMessage: "Value cannot be null.",
                    invalidTypeDiagnosticMessage: $"{nameof(NonNullAttribute)} can only be applied to members with reference types.",
                    getCondition: (expr, t) => SyntaxFactory.ParseExpression($"!global::System.Object.Equals({expr}, null)"),
                    isValidType: t => t.Type.IsReferenceType);
            }
        }

        private static Contract NonEmptyContract {
            get {
                return new Contract(
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
                    isValidType: t => t.Type.IsEnumerable());
            }
        }

        private static IEnumerable<Contract> BasicComparisonContracts {
            get {
                yield return CreateBasicComparisonContract(
                   name: "Positive",
                   attributeType: typeof(PositiveAttribute),
                   operatorLiteral: ">",
                   operatorDescription: "greater than");

                yield return CreateBasicComparisonContract(
                    name: "Negative",
                    attributeType: typeof(NegativeAttribute),
                    operatorLiteral: "<",
                    operatorDescription: "less than");

                yield return CreateBasicComparisonContract(
                     name: "NonPositive",
                     attributeType: typeof(NonPositiveAttribute),
                     operatorLiteral: "<=",
                     operatorDescription: "less than or equal to");

                yield return CreateBasicComparisonContract(
                     name: "NonNegative",
                     attributeType: typeof(NonNegativeAttribute),
                     operatorLiteral: ">=",
                     operatorDescription: "greater than or equal to");
            }
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

        #endregion
    }
}
