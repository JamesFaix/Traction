using System;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Precompiler {

    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NonNullAttribute : ContractAttribute {

        public NonNullAttribute() : base() { }

        public override bool IsValidType(TypeInfo typeInfo) {
            return !typeInfo.Type.IsValueType;
        }
    }
}
