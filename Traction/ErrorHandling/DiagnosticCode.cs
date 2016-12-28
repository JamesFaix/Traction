namespace Traction {

    public enum DiagnosticCode {
        //0-99 Info
        RewriteConfirmed = 0,
        //100-199 General error
        RewriteFailed = 100,
        //200-299 General contract misuse
        NoPostconditionsOnVoid = 200,
        NoPostconditionsOnIteratorBlocks = 201,
        NoPreconditionsOnInheritedMembers = 202,
        MembersCannotInheritContractsFromMultipleSources = 203,
        NoContractsOnPartialMembers = 204,
        NoContractsOnExternMemebrs = 205,
        //300-399 Specific contract misuse
        InvalidTypeForContract = 300
    }
}
