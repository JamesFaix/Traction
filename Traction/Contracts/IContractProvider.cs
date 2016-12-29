using System;
using System.Collections.Generic;

namespace Traction.Contracts {

    internal interface IContractProvider {

        IEnumerable<Contract> Contracts { get; }

        Contract this[string attributeTypeName] { get; }
    }
}
