namespace Traction.ExperimentApp {

    class NonNullAttributeTest {

        #region Properties 

        private string _unmarkedProperty;
        private string _readonlyProperty;
        private string _writeonlyProperty;

        public string UnmarkedProperty {
            get { return _unmarkedProperty; }
            set { _unmarkedProperty = value; }
        }

        [NonNull]
        public string ReadonlyProperty {
            get { return _readonlyProperty; }
        }

        [NonNull]
        public string WriteonlyProeprty {
            set { _writeonlyProperty = value; }
        }

        public string UnmarkedAutoproperty { get; set; }

       // [NonNull]
        public string ReadWriteAutoProperty { get; set; }

      //  [NonNull]
        public string ReadonlyAutoProperty { get; }
        
        #endregion

        #region Methods

        public void UnmarkedMethod() {

        }

        public void MethodWithMarkedParameter([NonNull] string text) {

        }

        [return: NonNull]
        public string MethodWithMarkedReturnType() {
            return "x";
        }

        [return: NonNull]
        public string MethodWithMarkedParameterAndReturnType([NonNull] string text) {
            return text;
        }

        public string MethodWithMultipleMarkedParameters([NonNull] string text, [NonNull] object obj) {
            return text + obj.ToString();
        }

        [return: NonNull]
        public string MethodWithMarkedReturnTypeAndMultipleReturnStatements(int n) {

            if (n > 0) {
                return "abc";
            }
            else {
                return "123";
            }
        }

        #endregion

        #region Operators

        //Unmarked operator
        public static int operator + (NonNullAttributeTest a, NonNullAttribute b) {
            return 1;
        }

        //Marked parameter
        public static int operator - ([NonNull] NonNullAttributeTest a, NonNullAttributeTest b) {
            return 1;
        }

        //Marked return value
        [return: NonNull]
        public static string operator * (NonNullAttributeTest a, NonNullAttributeTest b) {
            return "test";
        }

        #endregion

        #region Conversions

        //Unmarked conversion
        public static explicit operator int(NonNullAttributeTest a) {
            return 1;
        }

        //Marked parameter
        public static explicit operator long([NonNull] NonNullAttributeTest a) {
            return 1;
        }

        //Marked return value
        [return: NonNull]
        public static explicit operator string(NonNullAttributeTest a) {
            return "test";
        }

        #endregion
    }
}
