namespace Traction.Demo {

    public class NegativeConsumer {

        #region Properties
        public int _normalProperty;
        public int _readWritePropertyWithContract;

        public int NormalReadWriteProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [Negative]
        public int ContractReadWriteProperty {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }

        #endregion

        #region Methods
        public void NormalMethod() {

        }

        public void PreconditionMethod([Negative] int value) {

        }

        [return: Negative]
        public int PostconditionMethod(int value) {
            return value;
        }

        [return: Negative]
        public int PreAndPostconditionMethod([Negative] int value) {
            return value + 1;
        }

        public int MultiplePreconditionsMethod([Negative] int value, [Negative] double value2) {
            return (int)(value + value2);
        }
        #endregion

        #region Nullable types



        public void PreconditionNullableMethod([Negative] int? value) {

        }

        [return: Negative]
        public int? PostconditionNullableMethod(int? value) {
            return value;
        }

        #endregion
    }
}
