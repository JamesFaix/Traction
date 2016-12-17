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
            id: "TR0006",
            title: $"Invalid contract attribute usage",
            message: InvalidTypeDiagnosticMessage,
            location: location);

        protected abstract string InvalidTypeDiagnosticMessage { get; }
        
        public abstract bool IsDeclaredOn(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOn(PropertyDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOn(ParameterSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOnParameterOf(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOnReturnValueOf(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOrInheritedOn(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOrInheritedOn(PropertyDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOrInheritedOn(ParameterSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOrInheritedOnParameterOf(BaseMethodDeclarationSyntax node, SemanticModel model);

        public abstract bool IsDeclaredOrInheritedOnReturnValueOf(BaseMethodDeclarationSyntax node, SemanticModel model);
    }

    public abstract class Contract<TAttribute> : Contract
        where TAttribute : ContractAttribute {

        public override bool IsDeclaredOn(ParameterSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);        

        public override bool IsDeclaredOn(PropertyDeclarationSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);

        public override bool IsDeclaredOnParameterOf(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.ParameterList.Parameters.Any(p => IsDeclaredOn(p, model));

        public override bool IsDeclaredOnReturnValueOf(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.HasAttribute<TAttribute>(model);

        public override bool IsDeclaredOn(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            IsDeclaredOnReturnValueOf(node, model) || IsDeclaredOnParameterOf(node, model);


        public override bool IsDeclaredOrInheritedOn(ParameterSyntax node, SemanticModel model) =>
           node.HasOrInheritsAttribute<TAttribute>(model);
        
        public override bool IsDeclaredOrInheritedOn(PropertyDeclarationSyntax node, SemanticModel model) =>
            node.HasOrInheritsAttribute<TAttribute>(model);

        public override bool IsDeclaredOrInheritedOnParameterOf(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.ParameterList.Parameters.Any(p => IsDeclaredOrInheritedOn(p, model));

        public override bool IsDeclaredOrInheritedOnReturnValueOf(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            node.HasOrInheritsAttribute<TAttribute>(model);

        public override bool IsDeclaredOrInheritedOn(BaseMethodDeclarationSyntax node, SemanticModel model) =>
            IsDeclaredOrInheritedOnReturnValueOf(node, model) || IsDeclaredOrInheritedOnParameterOf(node, model);

    }
}
