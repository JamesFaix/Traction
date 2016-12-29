using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Traction.DiagnosticsTests {

    static class DiagnosticExtensions {

        static DiagnosticExtensions() {
            diagnosticCodeRegex = new Regex(@"TR(\d{4})");            
        }

        private static readonly Regex diagnosticCodeRegex;

        public static int GetCode(this Diagnostic @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            Match match = diagnosticCodeRegex.Match(@this.Descriptor.Id);

            if (!match.Success) {
                throw new InvalidOperationException("Diagnostic does not have Traction diagnostic code.");
            }

            string digits = match.Groups[1].Value; //Group 0 will be the entire match
            return int.Parse(digits);
        }

        public static string GetTitle(this Diagnostic @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Descriptor.Title.ToString();
        }

        public static string GetMessage(this Diagnostic @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Descriptor.MessageFormat.ToString();
        }
        
        public static bool ContainsCode(this IEnumerable<Diagnostic> @this, int code) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Any(d => d.GetCode() == code);
        }

        public static bool DoesNotContainCode(this IEnumerable<Diagnostic> @this, int code) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return !@this.Any(d => d.GetCode() == code);
        }

        public static bool ContainsOnlyCode(this IEnumerable<Diagnostic> @this, int code) {
            return @this.All(d => d.GetCode() == code);
        }
    }
}
