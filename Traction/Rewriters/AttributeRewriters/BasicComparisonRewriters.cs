using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Traction {

    abstract class BasicComparisonRewriter<TAttribute> : ContractRewriterBase<TAttribute>
        where TAttribute : ContractAttribute {

        protected BasicComparisonRewriter(SemanticModel model, ICompileContext context) : base(model, context) { }

        protected sealed override string ExceptionMessage => $"Value must be {OperatorDescription} default(T).";

        protected sealed override ExpressionSyntax GetConditionExpression(string expression, TypeInfo expressionType) {
            var symbol = expressionType.Type;
            var typeName = symbol.FullName();

            string text;

            if (symbol.IsNullable()) { //If type is nullable (T?), compare to default(T) instead of default(T?)
                typeName = (symbol as INamedTypeSymbol).TypeArguments[0].FullName();
                text = $"{expression}.HasValue && {expression}.Value.CompareTo(default({typeName})) {Operator} 0";
            }
            else {
                text = $"{expression}.CompareTo(default({typeName})) {Operator} 0";
            }

            return SyntaxFactory.ParseExpression(text);
        }

        protected abstract string Operator { get; }
        protected abstract string OperatorDescription { get; }

        //Applies to all types
        protected sealed override bool IsValidType(TypeInfo type) {
            var symbol = type.Type;

            if (symbol.IsNullable()) {
                return (symbol as INamedTypeSymbol).TypeArguments[0].IsComparable();
            }
            else {
                return symbol.IsValueType && symbol.IsComparable();
            }
        }

        protected sealed override Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Incorrect attribute usage",
            message: $"Basic comparison attributes can only be applied to members with value types implementing IComparable<T>, " +
                "or nullable types whose underlying type implements IComaprable<T>." +
                "(Since the default value of all reference types is null, a comparison with default(T) would just be a null check.)",
            location: location);
    }

    sealed class PositiveRewriter : BasicComparisonRewriter<PositiveAttribute> {
        public PositiveRewriter(SemanticModel model, ICompileContext context) : base(model, context) { }

        protected override string Operator => ">";
        protected override string OperatorDescription => "greater than";
    }

    sealed class NegativeRewriter : BasicComparisonRewriter<NegativeAttribute> {
        public NegativeRewriter(SemanticModel model, ICompileContext context) : base(model, context) { }

        protected override string Operator => "<";
        protected override string OperatorDescription => "less than";
    }

    sealed class NonPositiveRewriter : BasicComparisonRewriter<NonPositiveAttribute> {
        public NonPositiveRewriter(SemanticModel model, ICompileContext context) : base(model, context) { }

        protected override string Operator => "<=";
        protected override string OperatorDescription => "less than or equal to";
    }

    sealed class NonNegativeRewriter : BasicComparisonRewriter<NonNegativeAttribute> {
        public NonNegativeRewriter(SemanticModel model, ICompileContext context) : base(model, context) { }

        protected override string Operator => ">=";
        protected override string OperatorDescription => "greater than or equal to";
    }
}
