using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts.Semantics;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;
using Traction.SEPrecompilation;
using Traction.Roslyn.Rewriting;

namespace Traction.Contracts {

    internal class Contract {

        internal Contract(
            string name,
            Type attributeType,
            string exceptionMessage,
            string invalidTypeDiagnosticMessage,
            Func<string, ITypeSymbol, ExpressionSyntax> getCondition,
            Func<ITypeSymbol, bool> isValidType) {

            if (name == null) throw new ArgumentNullException(nameof(name));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            if (exceptionMessage == null) throw new ArgumentNullException(nameof(exceptionMessage));
            if (invalidTypeDiagnosticMessage == null) throw new ArgumentNullException(nameof(invalidTypeDiagnosticMessage));
            if (getCondition == null) throw new ArgumentNullException(nameof(getCondition));
            if (isValidType == null) throw new ArgumentNullException(nameof(isValidType));
            if (!attributeType.IsSubclassOf(typeof(ContractAttribute)))
                throw new ArgumentException($"Attribute type must inherit from {nameof(ContractAttribute)}", nameof(attributeType));

            Name = name;
            AttributeType = attributeType;
            ExceptionMessage = exceptionMessage;
            InvalidTypeDiagnosticMessage = invalidTypeDiagnosticMessage;
            this.getCondition = getCondition;
            this.isValidType = isValidType;
        }
        
        private readonly Func<string, ITypeSymbol, ExpressionSyntax> getCondition;
        private readonly Func<ITypeSymbol, bool> isValidType;

        public string Name { get; }
        public Type AttributeType { get; }
        public string ExceptionMessage { get; }
        public string InvalidTypeDiagnosticMessage { get; }

        public bool IsValidType(ITypeSymbol type) => isValidType(type);
        
        public ExpressionSyntax GetCondition(string expression, ITypeSymbol expressionType) => 
            this.getCondition(expression, expressionType);

        public override string ToString() => $"Contract: {Name}, {ExceptionMessage}";
    }
}
