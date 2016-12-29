namespace Traction {    
    //TODO: Make error codes more resemble HTTP codes?

    public enum DiagnosticCode {
        //Leave 0 as default for unassigned values
        Default = 0, 

        //1-99 Info
        RewriteConfirmed = 1,
       
        //100-199 General error
        RewriteFailed = 100,
        
        //200-299 General contract misuse
        NoPostconditionsOnVoid = 200,
        NoPostconditionsOnIteratorBlocks = 201,
        NoPreconditionsOnInheritedMembers = 202,
        MembersCannotInheritPreconditionsFromMultipleSources = 203,
        NoContractsOnPartialMembers = 204,
        NoContractsOnExternMemebrs = 205,
        
        //300-399 Specific contract misuse
        InvalidTypeForContract = 300
    }
}
