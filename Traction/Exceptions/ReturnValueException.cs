using System;
using System.Runtime.Serialization;

namespace Traction {

    [Serializable]
    public class ReturnValueException : Exception {

        public ReturnValueException() : base() { }

        public ReturnValueException(string message) : base(message) { }

        public ReturnValueException(string message, Exception innerException) : base(message, innerException) { }

        protected ReturnValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
