using System.Collections;
using System;

namespace Traction {

    public class NonNullConsumer {

        #region Properties 

        public string _normalProperty;
        public string _readonlyPropertyWithContract;
        public string _writeonlyPropertyWithContract;
        public string _readWritePropertyWithContract;
        public string _returnValue1;
        public string _returnValue2;
        private bool _toggle;
        
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

        //public string NormalAutoProperty { get; set; }

        //// [NonNull]
        //public string ReadWriteAutoProperty { get; set; }

        ////  [NonNull]
        //public string ReadonlyAutoProperty { get; }

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

        #endregion

        //#region Operators

        ////Normal operator
        //public static int operator +(NonNullAttributeTest a, NonNullAttribute b) {
        //    return 1;
        //}

        ////Precondition
        //public static int operator -([NonNull] NonNullAttributeTest a, NonNullAttributeTest b) {
        //    return 1;
        //}

        ////Postcondition
        //[return: NonNull]
        //public static string operator *(NonNullAttributeTest a, NonNullAttributeTest b) {
        //    return "test";
        //}

        ////Pre & Postcondition
        //[return: NonNull]
        //public static string operator /([NonNull] NonNullAttributeTest a, NonNullAttributeTest b) {
        //    return "test";
        //}

        ////Postcondition and multiple returns
        //[return: NonNull]
        //public static string operator %(NonNullAttributeTest a, NonNullAttributeTest b) {
        //    if (Equals(a, b)) {
        //        return "ABC";
        //    }
        //    else {
        //        return "123";
        //    }
        //}

        //#endregion

        //#region Conversions

        ////Normal conversion
        //public static explicit operator int(NonNullAttributeTest a) {
        //    return 1;
        //}

        ////Precondition
        //public static explicit operator long([NonNull] NonNullAttributeTest a) {
        //    return 1;
        //}

        ////Postcondition
        //[return: NonNull]
        //public static explicit operator string(NonNullAttributeTest a) {
        //    return "test";
        //}

        ////Pre & Postcondition
        //[return: NonNull]
        //public static explicit operator ArrayList([NonNull] NonNullAttributeTest a) {
        //    return new ArrayList();
        //}

        //#endregion
    }
}
