using System;

namespace Traction.Demo {

    /// <summary>
    /// Class to demonstrate NonNull contracts being applied to basic method and property use cases.
    /// </summary>
    public class NonNullConsumer {
        
        #region Properties 

        public string _normalProperty;
        public string _readonlyPropertyWithContract;
        public string _writeonlyPropertyWithContract;
        public string _readWritePropertyWithContract;

        public string NormalReadWriteProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [NonNull]
        public string ContractReadonlyProperty {
            get { return _readonlyPropertyWithContract; }
        }

        [NonNull]
        public string ContractWriteonlyProperty {
            set { _writeonlyPropertyWithContract = value; }
        }

        [NonNull]
        public string ContractReadWriteProperty {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }
        
        #endregion

        #region Methods

        public void NormalMethod() {

        }

        public void PreconditionMethod([NonNull] string text) {

        }

        [return: NonNull]
        public string PostconditionMethod(string text) {
            return text;
        }

        [return: NonNull]
        public string PreAndPostconditionMethod([NonNull] Func<string> textGenerator) {
            return textGenerator();
        }

        public string MultiplePreconditionsMethod([NonNull] string text, [NonNull] object obj) {
            return text + obj.ToString();
        }

        #endregion

        #region Nullable types

        public void PreconditionNullableMethod([NonNull] int? index) {

        }

        [return: NonNull]
        public int? PostconditionNullableMethod(int? index) {
            return index;
        }
        #endregion
    }
}
