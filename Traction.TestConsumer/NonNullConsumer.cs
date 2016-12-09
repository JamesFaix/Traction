using System;
using System.Collections;

namespace Traction {

    public class NonNullConsumer {

        public string _returnValue1;
        public string _returnValue2;
        private bool _toggle;

        #region Properties 

        public string _normalProperty;
        public string _readonlyPropertyWithContract;
        public string _writeonlyPropertyWithContract;
        public string _readWritePropertyWithContract;

        public string NormalProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [NonNull]
        public string ReadonlyPropertyWithContract {
            get { return _readonlyPropertyWithContract; }
        }

        [NonNull]
        public string WriteonlyPropertyWithContract {
            set { _writeonlyPropertyWithContract = value; }
        }

        [NonNull]
        public string ReadWritePropertyWithContract {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }

        [NonNull]
        public string ReadonlyPropertyWithContractAndMultipleReturns {
            get {
                _toggle = !_toggle;
                if (_toggle) {
                    return _returnValue1;
                }
                else {
                    return _returnValue2;
                }
            }
        }

        [NonNull]
        public string ReadonlyAutoproperty { get; }

        [NonNull]
        public string ReadWriteAutoproperty { get; set; }

        [NonNull]
        public string ReadonlyExpressionBodiedProperty => null;

        #endregion

        #region Methods

        public void NormalMethod() {

        }

        public void MethodWithPrecondition([NonNull] string text) {

        }

        [return: NonNull]
        public string MethodWithPostcondition(string text) {
            return text;
        }

        [return: NonNull]
        public string MethodWithPreAndPostcondition([NonNull] Func<string> textGenerator) {
            return textGenerator();
        }

        public string MethodWithMultiplePreconditions([NonNull] string text, [NonNull] object obj) {
            return text + obj.ToString();
        }

        [return: NonNull]
        public string MethodWithPostconditionAndMultipleReturns() {
            _toggle = !_toggle;
            if (_toggle) {
                return _returnValue1;
            }
            else {
                return _returnValue2;
            }
        }

        [return: NonNull]
        public string ExpressionBodiedMethodWithPostcondition() => null;

        #endregion

        #region Operators

        //Normal operator
        public static int operator +(NonNullConsumer a, NonNullConsumer b) {
            return 1;
        }

        //Precondition
        public static int operator -([NonNull] NonNullConsumer a, NonNullConsumer b) {
            return 1;
        }

        //Postcondition
        [return: NonNull]
        public static string operator *(NonNullConsumer a, NonNullConsumer b) {
            if (a == null || b == null) return null;
            return "test";
        }

        //Pre & Postcondition
        [return: NonNull]
        public static string operator /([NonNull] NonNullConsumer a, NonNullConsumer b) {
            var result = Equals(a, b) ? null : "test";
            return result;
        }

        //Postcondition and multiple returns
        [return: NonNull]
        public static string operator %(NonNullConsumer a, NonNullConsumer b) {
            if (Equals(a, b)) {
                return a._returnValue1;
            }
            else {
                return a._returnValue2;
            }
        }

        #endregion

        #region Conversions

        //Normal conversion
        public static explicit operator int(NonNullConsumer a) {
            return 1;
        }

        //Precondition
        public static explicit operator long([NonNull] NonNullConsumer a) {
            return 1;
        }

        //Postcondition
        [return: NonNull]
        public static explicit operator string(NonNullConsumer a) {
            var result = (a == null) ? null : "test";
            return result;
        }

        //Pre & Postcondition
        [return: NonNull]
        public static explicit operator ArrayList([NonNull] NonNullConsumer a) {
            return a._returnValue1 == null ? null : new ArrayList();
        }

        //Post with multiple returns
        [return: NonNull]
        public static explicit operator int[] (NonNullConsumer a) {
            if (a._returnValue1 == null) {
                var size = a._returnValue2.Length;
                return (size > 0) ? new int[size] : null;
            }
            else {
                var size = a._returnValue1.Length;
                return (size > 0) ? new int[size] : null;
            }
        }

        #endregion

        #region Nested/Anonymous methods

        public string NormalMethodWithAnonymousMethod() {
            Func<string> anonymousMethod = () => {
                return null;
            };

            return "test";
        }

        [return: NonNull]
        public string MethodWithAnonymousMethodAndPostcondition() {

            Func<string> anonymousMethod = () => {
                return null;
            };

            return "test";
        }

        #endregion
    }
}
