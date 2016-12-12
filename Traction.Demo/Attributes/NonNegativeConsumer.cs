namespace Traction.Demo {

    public class NonNegativeConsumer {

        #region Properties
        public int _normalProperty;
        public int _readWritePropertyWithContract;

        public int NormalReadWriteProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [NonNegative]
        public int ContractReadWriteProperty {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }

        #endregion

        #region Methods
        public void NormalMethod() {

        }

        public void PreconditionMethod([NonNegative] int value) {

        }

        [return: NonNegative]
        public int PostconditionMethod(int value) {
            return value;
        }

        [return: NonNegative]
        public int PreAndPostconditionMethod([NonNegative] int value) {
            return value - 1;
        }

        public int MultiplePreconditionsMethod([NonNegative] int value, [NonNegative] double value2) {
            return (int)(value + value2);
        }
        #endregion

        #region Nullable types



        public void PreconditionNullableMethod([NonNegative] int? value) {

        }

        [return: NonNegative]
        public int? PostconditionNullableMethod(int? value) {
            return value;
        }

        #endregion
    }
}
