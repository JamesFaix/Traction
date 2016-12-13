namespace Traction.Demo {

    public class NonPositiveConsumer {

        #region Properties
        public int _normalProperty;
        public int _readWritePropertyWithContract;

        public int NormalReadWriteProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [NonPositive]
        public int ContractReadWriteProperty {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }

        #endregion

        #region Methods
        public void NormalMethod() {

        }

        public void PreconditionMethod([NonPositive] int value) {

        }

        [return: NonPositive]
        public int PostconditionMethod(int value) {
            return value;
        }

        [return: NonPositive]
        public int PreAndPostconditionMethod([NonPositive] int value) {
            return value + 1;
        }

        public int MultiplePreconditionsMethod([NonPositive] int value, [NonPositive] double value2) {
            return (int)(value + value2);
        }
        #endregion

        #region Nullable types



        public void PreconditionNullableMethod([NonPositive] int? value) {

        }

        [return: NonPositive]
        public int? PostconditionNullableMethod(int? value) {
            return value;
        }

        #endregion
    }
}
