using System;
using System.Collections.Generic;
using System.Text;

namespace Traction {

    static class EnumerableExtensions {

        public static string ToDelimitedString<T>(this IEnumerable<T> @this, string delimiter) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));

            var skipDelimiter = true;
            var sb = new StringBuilder();

            foreach (var item in @this) {
                if (skipDelimiter) {
                    sb.Append(item);
                    skipDelimiter = false;
                }
                else {
                    sb.Append(delimiter + item);
                }
            }

            return sb.ToString();
        }

    }
}
