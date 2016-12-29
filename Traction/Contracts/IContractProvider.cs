using System;
using System.Collections.Generic;

namespace Traction.Contracts {

    internal interface IContractProvider {

        IEnumerable<Contract> Contracts { get; }

        Contract this[Type attributeType] { get; }
    }
}
