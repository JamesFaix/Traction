using System;
using System.Runtime.Serialization;

namespace Traction {

    /// <summary>
    /// Exception to throw when the return value of a method is invalid. (Postcondition assertion.)
    /// </summary>
    [Serializable]
    public class ReturnValueException : Exception {

        public ReturnValueException() : base() { }

        public ReturnValueException(string message) : base(message) { }

        public ReturnValueException(string message, Exception innerException) : base(message, innerException) { }

        protected ReturnValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
