using System;
using System.Runtime.Serialization;

namespace Traction {

    [Obsolete("StackExchange.Precompilation doesn't seem to like reporting custom exception types."),
     Serializable]
    public class PrecompilationException : Exception {

        public PrecompilationException() : base() { }

        public PrecompilationException(string message) : base(message) { }

        public PrecompilationException(string message, Exception innerException) : base(message, innerException) { }

        protected PrecompilationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
