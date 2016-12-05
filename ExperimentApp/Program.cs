using System.Text;

namespace Traction.ExperimentApp {

    /// <summary>
    /// This application uses the StackExchange.Precompilation compiler instead of the normal C# compiler.
    /// To verify transformations, compare this project's output with that from ControlApp in a disassembler program like ILSpy.
    /// </summary>
    class Program {

        public static void Main() {

        }
        
        [return: NonNull]
        public static string DoStuff([NonNull] string text, int number) {
            var result = "hello";

            result += " world";

            return result;
        }

        private string name;

        [NonNull]
        public string Name {
            get { return name; }
            set { name = value; }
        }

        private int number;
        
        public int Number {
            get { return number; }
            set {
                number = value;
                return;
            }
        }

    }
}
