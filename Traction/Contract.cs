using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

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

            this.name = name;
            this.attributeType = attributeType;
            this.exceptionMessage = exceptionMessage;
            this.invalidTypeDiagnosticMessage = invalidTypeDiagnosticMessage;
            this.getCondition = getCondition;
            this.isValidType = isValidType;
        }

        private readonly string name;
        private readonly Type attributeType;
        private readonly string exceptionMessage;
        private readonly string invalidTypeDiagnosticMessage;
        private readonly Func<string, TypeInfo, ExpressionSyntax> getCondition;
        private readonly Func<TypeInfo, bool> isValidType;

        public string Name => name;
        public string ExceptionMessage => exceptionMessage;

        public Type AttributeType => attributeType;

        public bool IsValidType(TypeInfo type) => isValidType(type);

        public Diagnostic GetInvalidTypeDiagnostic(Location location) =>
            DiagnosticFactory.Create(
                code: DiagnosticCode.InvalidTypeForContract,
                title: $"Invalid contract attribute usage",
                message: this.invalidTypeDiagnosticMessage,
                location: location);

        public ExpressionSyntax GetCondition(string expression, TypeInfo expressionType) => 
            this.getCondition(expression, expressionType);
    }
}
