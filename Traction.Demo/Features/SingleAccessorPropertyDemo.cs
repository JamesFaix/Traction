namespace Traction.Demo {

    /// <summary>
    /// Class to demonstrate contracts being applied uniformly on readonly, writeonly, and read/write properties.
    /// </summary>
    public class SingleAccessorPropertyDemo {

        public string _normalPropertyField;
        public string _contractReadonlyProeprtyField;
        public string _contractWriteonlyPropertyField;
        public string _contractReadWritePropertyField;

        public string NormalReadWrite {
            get { return _normalPropertyField; }
            set { _normalPropertyField = value; }
        }

        [NonNull]
        public string ContractReadonly {
            get { return _contractReadonlyProeprtyField; }
        }

        [NonNull]
        public string ContractWriteonly {
            set { _contractWriteonlyPropertyField = value; }
        }

        [NonNull]
        public string ContractReadWrite {
            get { return _contractReadWritePropertyField; }
            set { _contractReadWritePropertyField = value; }
        }
    }
}
