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
    }
}
