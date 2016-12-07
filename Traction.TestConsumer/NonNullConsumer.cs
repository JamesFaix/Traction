using System.Collections;
using System;

namespace Traction {

    public class NonNullConsumer {

        #region Properties 

        public string _normalProperty;
        public string _readonlyPropertyWithContract;
        public string _writeonlyPropertyWithContract;
        public string _readWritePropertyWithContract;
        public string _readonlyPropertyWithContractAndMultipleReturns1;
        public string _readonlyPropertyWithContractAndMultipleReturns2;
        private bool _readonlyPropertyWithContractAndMultipleReturnsToggle;
        
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
                _readonlyPropertyWithContractAndMultipleReturnsToggle = !_readonlyPropertyWithContractAndMultipleReturnsToggle;
                if (_readonlyPropertyWithContractAndMultipleReturnsToggle) {
                    return _readonlyPropertyWithContractAndMultipleReturns1;
                }
                else {
                    return _readonlyPropertyWithContractAndMultipleReturns2;
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

        //public void NormalMethod() {

        //}

        //public void MethodWithPrecondition([NonNull] string text) {

        //}

        //[return: NonNull]
        //public string MethodWithPostcondition() {
        //    return "x";
        //}

        //[return: NonNull]
        //public string MethodWithPreAndPostcondition([NonNull] string text) {
        //    return text;
        //}

        //public string MethodWithMultiplePreconditions([NonNull] string text, [NonNull] object obj) {
        //    return text + obj.ToString();
        //}

        [return: NonNull]
        public string MethodWithPostconditionAndMultipleReturns(int n) {

            if (n > 0) {
                return "abc";
            }
            else {
                return "123";
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
