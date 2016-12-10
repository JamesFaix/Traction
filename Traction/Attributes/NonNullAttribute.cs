using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Contract requiring (or ensuring) that a value cannot be <c>null</c>.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NonNullAttribute : ContractAttribute {

        public NonNullAttribute() : base() { }

        public override bool IsValidType(TypeInfo typeInfo) {
            return !typeInfo.Type.IsValueType;
        }
    }
}
