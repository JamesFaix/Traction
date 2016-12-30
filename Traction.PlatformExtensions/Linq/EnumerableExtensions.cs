using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Traction.Linq {

    public static class EnumerableExtensions {

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

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> @this, T element) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Concat(Enumerable.Repeat(element, 1));
        }

        public static IEnumerable<T> Concat<T>(this T @this, IEnumerable<T> sequence) {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            return Enumerable.Repeat(@this, 1).Concat(sequence);
        }
    }
}
