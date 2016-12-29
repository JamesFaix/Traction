using System.Collections.Generic;

namespace Traction {

    interface IContractProvider {
        IEnumerable<Contract> Contracts { get; }
    }
}
