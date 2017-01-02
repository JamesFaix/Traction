using System;
using System.Text;

namespace Traction.Tests {

    internal static class StringBuilderExtensions {

        public static StringBuilder AppendIf(this StringBuilder @this, bool condition, string text) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (condition) @this.Append(text);
            return @this;
        }

        public static StringBuilder AppendLineIf(this StringBuilder @this, bool condition, string text) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (condition) @this.AppendLine(text);
            return @this;
        }
    }
}
