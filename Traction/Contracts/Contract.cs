using System.Linq;
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

        public Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Invalid contract attribute usage",
            message: InvalidTypeDiagnosticMessage,
            location: location);

        protected abstract string InvalidTypeDiagnosticMessage { get; }

        public abstract bool IsOn(ParameterSyntax node, SemanticModel model);

        public abstract bool IsOnParameterOf(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsOnReturnValueOf(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsOn(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsOn(PropertyDeclarationSyntax node, SemanticModel model);
    }

    public abstract class Contract<TAttribute> : Contract
        where TAttribute : ContractAttribute {

        public override bool IsOn(ParameterSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);

        public override bool IsOnParameterOf(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.ParameterList.Parameters.Any(p => IsOn(p, model));

        public override bool IsOnReturnValueOf(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);

        public override bool IsOn(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.HasAnyAttribute<TAttribute>(model);

        public override bool IsOn(PropertyDeclarationSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);
    }
}
