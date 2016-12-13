using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics.Contracts;

namespace Traction.Demo {

    public class NonEmptyConsumer {

        #region String

        #region Properties 

        public string _normalProperty;
        public string _readWritePropertyWithContract;

        public string NormalReadWriteProperty {
            get { return _normalProperty; }
            set { _normalProperty = value; }
        }

        [NonEmpty]
        public string ContractReadWriteProperty {
            get { return _readWritePropertyWithContract; }
            set { _readWritePropertyWithContract = value; }
        }

        #endregion

        #region Methods

        public void NormalMethod() {

        }

        public void PreconditionMethod([NonEmpty] string text) {

        }

        [return: NonEmpty]
        public string PostconditionMethod(string text) {
            return text;
        }

        [return: NonEmpty]
        public string PreAndPostconditionMethod([NonEmpty] string text) {
            return text.Length > 2 ? text : "";
        }

        public string MultiplePreconditionsMethod([NonEmpty] string text, [NonEmpty] string text2) {
            return text + text2;
        }

        #endregion

        #endregion

    }
}
