namespace Traction.Demo {

    public class PositiveConsumer {

        #region Properties
        public int _normalProperty;
        public int _readWritePropertyWithContract;

        public int NormalReadWriteProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [Positive]
        public int ContractReadWriteProperty {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }

        #endregion

        #region Methods
        public void NormalMethod() {

        }

        public void PreconditionMethod([Positive] int value) {

        }

        [return: Positive]
        public int PostconditionMethod(int value) {
            return value;
        }

        [return: Positive]
        public int PreAndPostconditionMethod([Positive] int value) {
            return value - 1;
        }

        public int MultiplePreconditionsMethod([Positive] int value, [Positive] double value2) {
            return (int)(value + value2);
        }
        #endregion

        #region Nullable types



        public void PreconditionNullableMethod([Positive] int? value) {

        }

        [return: Positive]
        public int? PostconditionNullableMethod(int? value) {
            return value;
        }

        #endregion
    }
}
