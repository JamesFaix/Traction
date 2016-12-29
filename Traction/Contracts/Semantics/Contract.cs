using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Contracts.Semantics {

    internal class Contract {

        internal Contract(
            string name,
            Type attributeType,
            string exceptionMessage,
            string invalidTypeDiagnosticMessage,
            Func<string, TypeInfo, ExpressionSyntax> getCondition,
            Func<TypeInfo, bool> isValidType) {

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
        
        private readonly Func<string, TypeInfo, ExpressionSyntax> getCondition;
        private readonly Func<TypeInfo, bool> isValidType;

        public string Name { get; }
        public Type AttributeType { get; }
        public string ExceptionMessage { get; }
        public string InvalidTypeDiagnosticMessage { get; }

        public bool IsValidType(TypeInfo type) => isValidType(type);
        
        public ExpressionSyntax GetCondition(string expression, TypeInfo expressionType) => 
            this.getCondition(expression, expressionType);
    }
}
