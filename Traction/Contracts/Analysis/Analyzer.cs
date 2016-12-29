using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Analysis {

    internal class Analyzer : CSharpSyntaxRewriter {

        public Analyzer(SemanticModel model, ICompileContext context, IContractProvider contractProvider) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            this.model = model;
            this.context = context;
            this.contractProvider = contractProvider;
        }

        private readonly ICompileContext context;
        private readonly SemanticModel model;
        private readonly IContractProvider contractProvider;
        
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        private SyntaxNode VisitBaseMethodDeclaration(BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var symbol = model.GetDeclaredSymbol(node) as IMethodSymbol;



            //Check for partial or extern

            //Check for Multiple pre w/inheritance

            //If has postcondition, check:
            //Post w/ void
            //Post w/ iterator
            //Invalid type

            return node;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var symbol = model.GetDeclaredSymbol(node) as IPropertySymbol;
            
            //Check for partial or extern
            
            //Check for getter and setter

            //Check for invalid type

            //Check setter:
            //Pre w/inheritance
            //Multiple pre

            //Check getter:
            //Post w/ iterator

            return node;
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node) {
            //Get member symbol
            
            //Check for:
                //Pre w/ inheritance
                //Invalid type

            return node;
        }
    }
}
