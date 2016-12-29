using System.Collections.Generic;

namespace Traction.Contracts {

    public interface IContractProvider {
        IEnumerable<Contract> Contracts { get; }
    }
}
