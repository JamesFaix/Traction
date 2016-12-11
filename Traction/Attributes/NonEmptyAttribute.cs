using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Contract requiring (or ensuring) that a collection has at least one element, or a string has at least one character.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class NonEmptyAttribute : ContractAttribute {

        public NonEmptyAttribute() : base() { }

    }
}
