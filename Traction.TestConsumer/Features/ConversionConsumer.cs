using System.Collections;

namespace Traction.TestConsumer {

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
