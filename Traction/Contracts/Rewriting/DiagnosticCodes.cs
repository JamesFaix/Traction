namespace Traction.Contracts.Rewriting {

    /// <summary>
    /// These extend the set of codes in Traction.Roslyn.Rewriting.DiagnosticCodes,
    /// which are in the range [0..199].
    /// </summary>
    public class DiagnosticCodes {

        //200-299 General contract misuse
        public const int NoPostconditionsOnVoid = 200;
        public const int NoPostconditionsOnIteratorBlocks = 201;
        public const int NoPreconditionsOnInheritedMembers = 202;
        public const int MembersCannotInheritPreconditionsFromMultipleSources = 203;
        public const int NoContractsOnPartialMembers = 204;
        public const int NoContractsOnExternMemebrs = 205;

        //300-399 Specific contract misuse
        public const int InvalidTypeForContract = 300;
    }
}