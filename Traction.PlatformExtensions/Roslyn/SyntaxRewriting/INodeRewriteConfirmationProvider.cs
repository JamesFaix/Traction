using Microsoft.CodeAnalysis;

namespace Traction.Roslyn {

    interface INodeRewriteConfirmationProvider {

        Diagnostic GetConfirmation(Location location);

    }
}
