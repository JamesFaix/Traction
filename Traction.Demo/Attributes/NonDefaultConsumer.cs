﻿using System;

namespace Traction.Demo {

    public class NonDefaultConsumer {

        #region Properties

        #region Reference types
        public string _normalReferenceTypeProperty;
        public string _contractReadWriteReferenceTypeProperty;

        public string NormalReadWriteReferenceTypeProperty {
            get { return _normalReferenceTypeProperty; }
            set { _normalReferenceTypeProperty = value; }
        }

        [NonDefault]
        public string ContractReadWriteReferenceTypeProperty {
            get { return _contractReadWriteReferenceTypeProperty; }
            set { _contractReadWriteReferenceTypeProperty = value; }
        }
        #endregion

        #region Value types
        public int _normalValueTypeProperty;
        public int _contractReadWriteValueTypeProperty;

        public int NormalReadWriteValueTypeProperty {
            get { return _normalValueTypeProperty; }
            set { _normalValueTypeProperty = value; }
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
            return name == "" ? null : name;
        }
        #endregion

        #region Value types

        public void PreconditionValueTypeMethod([NonDefault] int index) {

        }

        [return: NonDefault]
        public int PostconditionValueTypeMethod(int index) {
            return index;
        }

        [return: NonDefault]
        public int PreAndPostconditionValueTypeMethod([NonDefault] int index) {
            return index - 1;
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
        public Func<int> PostconditionGenericMethod(Func<int> function) {
            return function;
        }

        #endregion

        #region Nullable Types

        public void PreconditionNullableMethod([NonDefault] int? index) {

        }

        [return: NonDefault]
        public int? PostconditionNullableMethod(int? index) {
            return index;
        }

        #endregion
    }
}
