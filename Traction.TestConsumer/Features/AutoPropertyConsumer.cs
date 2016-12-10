namespace Traction.TestConsumer {

    /// <summary>
    /// Class to demonstrate contracts being applied to auto-properties.
    /// Correct auto-property behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal properties, and this class works correctly.
    /// </summary>
    public class AutoPropertyConsumer {

        public string NormalReadonly { get; }

        [NonNull]
        public string ContractReadonly { get; }

        public string NormalReadWrite { get; set; }

        [NonNull]
        public string ContractReadWrite { get; set; }

    }
}
