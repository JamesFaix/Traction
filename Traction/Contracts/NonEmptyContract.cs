using System.Linq;
using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Contract for <see cref="NonEmptyAttribute"/>.
    /// </summary>
    public class NonEmptyContract : Contract<NonEmptyAttribute> {

        public override string ExceptionMessage => "Sequence cannot be null or empty.";

        public override ExpressionSyntax Condition(string expression, TypeInfo expressionType) {
            var text = "";
            if (!expressionType.Type.IsValueType) { //Check for null if reference type.
                text += $"!global::System.Object.Equals({expression}, null) && ";
            }
            text += $"global::System.Linq.Enumerable.Any({expression})";
            return SyntaxFactory.ParseExpression(text);
        }

        //Applies to types implementing IEnumerable<T>
        public override bool IsValidType(TypeInfo type) => type.Type.IsEnumerable();

        protected override string InvalidTypeDiagnosticMessage =>
            $"{nameof(NonEmptyAttribute)} can only be applied to members with types implementing {nameof(IEnumerable)}<T>.";
    }
}