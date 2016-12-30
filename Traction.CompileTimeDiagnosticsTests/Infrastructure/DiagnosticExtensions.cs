using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction.DiagnosticsTests {

    static class DiagnosticExtensions {
        
        public static string GetTitle(this Diagnostic @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Descriptor.Title.ToString();
        }

        public static string GetMessage(this Diagnostic @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Descriptor.MessageFormat.ToString();
        }
        
        public static bool ContainsCode(this IEnumerable<Diagnostic> @this, string code) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Any(d => d.Id == code);
        }

        public static bool DoesNotContainCode(this IEnumerable<Diagnostic> @this, string code) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return !@this.Any(d => d.Id == code);
        }

        public static bool ContainsOnlyCode(this IEnumerable<Diagnostic> @this, string code) {
            return @this.All(d => d.Id == code);
        }
    }
}
