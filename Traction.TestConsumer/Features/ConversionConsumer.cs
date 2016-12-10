using System.Collections;

namespace Traction.TestConsumer {

    /// <summary>
    /// Class to demonstrate contracts being applied to conversion operators.
    /// The implementation for conversions is 99% the same as methods.
    /// Correct conversion operator behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods, and this class works correctly.
    /// </summary>
    public class ConversionConsumer {
        
        public string testField;
        
        //Normal conversion
        public static explicit operator int(ConversionConsumer a) {
            return 1;
        }

        //Precondition
        public static explicit operator long([NonNull] ConversionConsumer a) {
            return 1;
        }

        //Postcondition
        [return: NonNull]
        public static explicit operator string(ConversionConsumer a) {
            var result = (a == null) ? null : "test";
            return result;
        }

        //Pre & Postcondition
        [return: NonNull]
        public static explicit operator ArrayList([NonNull] ConversionConsumer a) {
            return a.testField == null ? null : new ArrayList();
        }
    }
}
