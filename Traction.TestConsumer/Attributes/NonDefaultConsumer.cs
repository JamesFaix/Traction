using System;

namespace Traction.TestConsumer {

    public class NonDefaultConsumer {

        #region Properties

        #region Reference types
        public string _normalReferenceTypeProperty;
        public string _contractReadonlyReferenceTypeProperty;
        public string _contractWriteonlyReferenceTypeProperty;
        public string _contractReadWriteReferenceTypeProperty;

        public string NormalReadWriteReferenceTypeProperty {
            get { return _normalReferenceTypeProperty; }
            set { _normalReferenceTypeProperty = value; }
        }

        [NonDefault]
        public string ContractReadonlyReferenceTypeProperty {
            get { return _contractReadonlyReferenceTypeProperty; }
        }

        [NonDefault]
        public string ContractWriteonlyReferenceTypeProperty {
            set { _contractWriteonlyReferenceTypeProperty = value; }
        }

        [NonDefault]
        public string ContractReadWriteReferenceTypeProperty {
            get { return _contractReadWriteReferenceTypeProperty; }
            set { _contractReadWriteReferenceTypeProperty = value; }
        }
        #endregion

        #region Value types
        public int _normalValueTypeProperty;
        public int _contractReadonlyValueTypeProperty;
        public int _contractWriteonlyValueTypeProperty;
        public int _contractReadWriteValueTypeProperty;

        public int NormalReadWriteValueTypeProperty {
            get { return _normalValueTypeProperty; }
            set { _normalValueTypeProperty = value; }
        }

        [NonDefault]
        public int ContractReadonlyValueTypeProperty {
            get { return _contractReadonlyValueTypeProperty; }
        }

        [NonDefault]
        public int ContractWriteonlyValueTypeProperty {
            set { _contractWriteonlyValueTypeProperty = value; }
        }

        [NonDefault]
        public int ContractReadWriteValueTypeProperty {
            get { return _contractReadWriteValueTypeProperty; }
            set { _contractReadWriteValueTypeProperty = value; }
        }
        #endregion
        #endregion

        #region Methods

        public void NormalMethod() {

        }
        
        #region Reference types
        
        public void PreconditionReferenceTypeMethod([NonDefault] string text) {

        }

        [return: NonDefault]
        public string PostconditionReferenceTypeMethod(string text) {
            return text;
        }

        [return: NonDefault]
        public string PreAndPostconditionReferenceTypeMethod([NonDefault] string name) {
            return null;
        }
        #endregion

        #region Value types

        public void PreconditionValueTypeMethod([NonDefault] int index) {

        }

        [return: NonDefault]
        public int PostconditionRefrenceTypeMethod(int index) {
            return index;
        }

        [return: NonDefault]
        public int PreAndPostconditionValueTypeMethod([NonDefault] int index) {
            return 0;
        }

        #endregion

        public int MultiplePreconditionsMethod([NonDefault] int index, [NonDefault] object obj) {
            return index + obj.ToString().Length;
        }
        #endregion

        #region Generic types

        public void PreconditionGenericMethod([NonDefault] Func<int> function) {

        }

        [return: NonDefault]
        public Func<int> PostconditionGenericMethod() {
            return null;
        }

        #endregion
    }
}
