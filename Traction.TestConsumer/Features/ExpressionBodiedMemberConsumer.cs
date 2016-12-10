namespace Traction.TestConsumer {

    public class ExpressionBodiedMemberConsumer {

        public string NormalProperty => null;

        [NonNull]
        public string ContractProperty => null;

        public string NormalMethod() => null;

        [return: NonNull]
        public string PostconditionMethod() => null;
    }
}
