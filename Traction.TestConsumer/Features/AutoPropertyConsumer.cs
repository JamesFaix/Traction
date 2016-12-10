namespace Traction.TestConsumer {

    public class AutoPropertyConsumer {

        public string NormalReadonly { get; }

        [NonNull]
        public string ContractReadonly { get; }

        public string NormalReadWrite { get; set; }

        [NonNull]
        public string ContractReadWrite { get; set; }

    }
}
