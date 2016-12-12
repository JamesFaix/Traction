using System.Diagnostics.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Contract requiring (or ensuring) that a value cannot be <c>null</c>.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NonNullAttribute : ContractAttribute {

        public NonNullAttribute() : base() { }
        
    }
}
