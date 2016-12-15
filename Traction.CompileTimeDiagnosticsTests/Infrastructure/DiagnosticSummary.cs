using System;
using Microsoft.CodeAnalysis;

namespace Traction.DiagnosticsTests {
    
    public class DiagnosticSummary : IEquatable<DiagnosticSummary> {

        public DiagnosticSummary(Diagnostic diagnostic) {
            if (diagnostic == null) throw new ArgumentNullException(nameof(diagnostic));

            var desc = diagnostic.Descriptor;

            Id = desc.Id;
            Title = desc.Title.ToString();
            Message = desc.MessageFormat.ToString();
        }

        public string Id { get; }

        public string Title { get; }

        public string Message { get; }

        #region Equality
        public bool Equals(DiagnosticSummary other) =>
            !Equals(other, null) &&
            Id == other.Id &&
            Title == other.Title &&
            Message == other.Message;

        public override bool Equals(object obj) =>
            Equals(obj as DiagnosticSummary);

        public override int GetHashCode() =>
            Message.GetHashCode();
        #endregion
    }
}
