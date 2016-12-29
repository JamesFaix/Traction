using System;
using System.Runtime.Serialization;

namespace Traction {

    /// <summary>
    /// Exception to throw when the return value of a method is invalid. (Postcondition assertion.)
    /// </summary>
    [Serializable]
    public class PostconditionException : Exception {

        public PostconditionException() : base() { }

        public PostconditionException(string message) : base(message) { }

        public PostconditionException(string message, Exception innerException) : base(message, innerException) { }

        protected PostconditionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
