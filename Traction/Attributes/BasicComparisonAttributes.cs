using System.Diagnostics.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Contract requiring (or ensuring) that a value is greater than the default of its type.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class PositiveAttribute : ContractAttribute {

        public PositiveAttribute() : base() { }

    }

    /// <summary>
    /// Contract requiring (or ensuring) that a value is less than the default of its type.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NegativeAttribute : ContractAttribute {

        public NegativeAttribute() : base() { }

    }

    /// <summary>
    /// Contract requiring (or ensuring) that a value is greater than or equal to the default of its type.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NonPositiveAttribute : ContractAttribute {

        public NonPositiveAttribute() : base() { }

    }

    /// <summary>
    /// Contract requiring (or ensuring) that a value is less than or equal to the default of its type.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NonNegativeAttribute : ContractAttribute {

        public NonNegativeAttribute() : base() { }

    }
}
