namespace Traction.Contracts.Analysis {

    /// <summary>
    /// These extend the set of codes in Traction.Roslyn.Rewriting.DiagnosticCodes,
    /// which are in the range [0..199].
    /// </summary>
    public class DiagnosticCodes {

        //200-299 General contract misuse
        public const string NoPostconditionsOnVoid = "TR0200";
        public const string NoPostconditionsOnIteratorBlocks = "TR0201";
        public const string NoPreconditionsOnInheritedMembers = "TR0202";
        public const string MembersCannotInheritPreconditionsFromMultipleSources = "TR0203";
        public const string NoContractsOnPartialMembers = "TR0204";
        public const string NoContractsOnExternMembers = "TR0205";

        //300-399 Specific contract misuse
        public const string InvalidTypeForContract = "TR0300";
    }
}