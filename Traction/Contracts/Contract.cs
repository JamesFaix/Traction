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

        public abstract bool HasAttribute(ParameterSyntax node, SemanticModel model);

        public abstract bool HasReturnValueAttribute(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool HasParameterOrReturnValueAttribute(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool HasAttribute(PropertyDeclarationSyntax node, SemanticModel model);
    }

    public abstract class Contract<TAttribute> : Contract
        where TAttribute : ContractAttribute {

        public override bool HasAttribute(ParameterSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);

        public override bool HasReturnValueAttribute(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);

        public override bool HasParameterOrReturnValueAttribute(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.HasAnyAttribute<TAttribute>(model);

        public override bool HasAttribute(PropertyDeclarationSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);
    }
}
