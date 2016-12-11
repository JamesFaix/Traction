using System;
using System.Runtime.Serialization;

namespace Traction {

    /// <summary>
    /// Exception to throw when the return value of a method is invalid. (Postcondition assertion.)
    /// </summary>
    [Serializable]
    public class PreconditionException : Exception {

        public PreconditionException() : base() { }

        public PreconditionException(string message) : base(message) { }
        
        public PreconditionException(string message, string parameterName) : base(message + ", " + parameterName) { }

        public PreconditionException(string message, Exception innerException) : base(message, innerException) { }

        protected PreconditionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
