using System.Text;

namespace Traction.ExperimentApp {

    class Program {

        public static void Main() {

        }
        
        public static int DoStuff([NonNull] string text, int number) {
            return 1;
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
