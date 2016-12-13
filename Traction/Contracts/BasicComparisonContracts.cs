using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    public abstract class BasicComparisonContract<TAttribute> : Contract
        where TAttribute : ContractAttribute {

        public sealed override string ExceptionMessage => $"Value must be {OperatorDescription} default(T).";

        public sealed override ExpressionSyntax Condition(string expression, TypeInfo expressionType) {
            var symbol = expressionType.Type;            
            string text;

            if (symbol.IsNullable()) { //If type is nullable (T?), compare to default(T) instead of default(T?)
                var typeName = (symbol as INamedTypeSymbol).TypeArguments[0].FullName();
                text = $"!{expression}.HasValue || {expression}.Value.CompareTo(default({typeName})) {Operator} 0";
            }
            else {
                var typeName = symbol.FullName();
                text = $"{expression}.CompareTo(default({typeName})) {Operator} 0";
            }

            return SyntaxFactory.ParseExpression(text);
        }

        protected abstract string Operator { get; }
        protected abstract string OperatorDescription { get; }

        //Applies to all types
        public sealed override bool IsValidType(TypeInfo type) {
            var symbol = type.Type;

            if (symbol.IsNullable()) {
                return (symbol as INamedTypeSymbol).TypeArguments[0].IsComparable();
            }
            else {
                return symbol.IsValueType && symbol.IsComparable();
            }
        }

        public sealed override Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Incorrect attribute usage",
            message: $"Basic comparison attributes can only be applied to members with value types implementing IComparable<T>, " +
                "or nullable types whose underlying type implements IComaprable<T>." +
                "(Since the default value of all reference types is null, a comparison with default(T) would just be a null check.)",
            location: location);
    }

    public sealed class PositiveContract : BasicComparisonContract<PositiveAttribute> {
        protected override string Operator => ">";
        protected override string OperatorDescription => "greater than";
    }

    public sealed class NegativeContract : BasicComparisonContract<NegativeAttribute> {
        protected override string Operator => "<";
        protected override string OperatorDescription => "less than";
    }

    public sealed class NonPositiveContract : BasicComparisonContract<NonPositiveAttribute> {
        protected override string Operator => "<=";
        protected override string OperatorDescription => "less than or equal to";
    }

    public sealed class NonNegativeContract : BasicComparisonContract<NonNegativeAttribute> {
        protected override string Operator => ">=";
        protected override string OperatorDescription => "greater than or equal to";
    }
}
