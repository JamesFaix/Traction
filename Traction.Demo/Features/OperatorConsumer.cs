namespace Traction.Demo {
    
    /// <summary>
    /// Class to demonstrate contracts being applied to operators.
    /// The implementation for operators is 99% the same as methods.
    /// Correct operator behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods, and this class works correctly.
    /// </summary>
    public class OperatorConsumer {
        
        //Normal operator
        public static int operator +(OperatorConsumer a, OperatorConsumer b) {
            return 1;
        }

        //Precondition
        public static int operator -([NonNull] OperatorConsumer a, [NonNull] OperatorConsumer b) {
            return 1;
        }

        //Postcondition
        [return: NonNull]
        public static string operator *(OperatorConsumer a, OperatorConsumer b) {
            if (a == null || b == null) return null;
            return "test";
        }

        //Pre & Postcondition
        [return: NonNull]
        public static string operator /([NonNull] OperatorConsumer a, [NonNull] OperatorConsumer b) {
            var result = Equals(a, b) ? null : "test";
            return result;
        }
    }
}
