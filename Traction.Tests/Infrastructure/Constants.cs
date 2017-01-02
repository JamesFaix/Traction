namespace Traction.Tests {

    internal static class Constants {

        public const string Normal = "DoesNotThrow";
        public const string Passes = "DoesNotThrowIfContractMet";
        public const string Fails = "ThrowsIfContractBroken";

        public static readonly object[] EmptyArgs = new object[0];
    }
}
