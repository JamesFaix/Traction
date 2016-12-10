namespace Traction.TestConsumer {

    /// <summary>
    /// Class to demonstrate contracts being applied to expression-bodied members.
    /// Correct expression-bodied member behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal properties and methods, and this class works correctly.
    /// </summary>
    public class ExpressionBodiedMemberConsumer {

        public string NormalProperty => null;

        [NonNull]
        public string ContractProperty => null;

        public string NormalMethod() => null;

        [return: NonNull]
        public string PostconditionMethod() => null;
    }
}
