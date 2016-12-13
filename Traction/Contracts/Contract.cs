using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    public abstract class Contract {

        /// <summary>
        /// Gets boolean expression to evaluate.  Should return false if contract is broken.
        /// </summary>
        public abstract ExpressionSyntax Condition(string expression, TypeInfo expressionType);

        public abstract string ExceptionMessage { get; }

        public abstract bool IsValidType(TypeInfo type);

        public abstract Diagnostic InvalidTypeDiagnostic(Location location);
    }
}
