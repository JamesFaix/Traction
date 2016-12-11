using System;

namespace Traction.Demo {

    /// <summary>
    /// Class to demonstrate postconditions being applied to members with multiple return statements or nested methods.
    /// Correct multiple-return-statement behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods/properties, and this class works correctly.
    /// </summary>
    public class MultipleReturnsConsumer {

        public string _returnValue1;
        public string _returnValue2;
        private bool _toggle;

        #region Multiple returns

        [return: NonNull]
        public string PostconditionMethod() {
            _toggle = !_toggle;
            if (_toggle) {
                return _returnValue1;
            }
            else {
                return _returnValue2;
            }
        }

        [NonNull]
        public string ContractProperty {
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

        #endregion

        #region Statement lambda expressions

        public string NormalMethodWithStatementLambda() {
            Func<string> anonymousMethod = () => {
                return null;
            };

            return "test";
        }

        [return: NonNull]
        public string PostconditionMethodWithStatementLambda() {

            Func<string> anonymousMethod = () => {
                return null;
            };

            return "test";
        }

        #endregion
    }
}
